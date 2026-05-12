using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TMPSmartWrappedLayout : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;

    [Header("폰트 크기 범위")]
    [SerializeField] private float maxFontSize = 48f;
    [SerializeField] private float minFontSize = 20f;
    [SerializeField] private float fontSearchStep = 1f;

    [Header("여백")]
    [SerializeField] private float widthPadding = 8f;
    [SerializeField] private float heightPadding = 8f;

    [Header("옵션")]
    [SerializeField] private bool disableAutoSize = true;
    [SerializeField] private bool disableWordWrapping = true;

    public TMP_Text TargetText => targetText;

    private void Reset()
    {
        targetText = GetComponent<TMP_Text>();
    }

    public string ApplyBestLayout(string rawText)
    {
        if (targetText == null)
        {
            Debug.LogWarning("[TMPSmartWrappedLayout] targetText가 연결되지 않았습니다.");
            return rawText ?? "";
        }

        RectTransform rect = targetText.rectTransform;
        float availableWidth = Mathf.Max(1f, rect.rect.width - widthPadding);
        float availableHeight = Mathf.Max(1f, rect.rect.height - heightPadding);

        float originalFontSize = targetText.fontSize;
        bool originalAutoSize = targetText.enableAutoSizing;

        if (disableAutoSize)
            targetText.enableAutoSizing = false;

        string bestText = rawText ?? "";
        float bestSize = minFontSize;

        for (float size = maxFontSize; size >= minFontSize; size -= fontSearchStep)
        {
            targetText.fontSize = size;

            string wrapped = WrapTextByGroup(rawText ?? "", availableWidth);
            Vector2 preferred = targetText.GetPreferredValues(wrapped, availableWidth, Mathf.Infinity);

            if (preferred.y <= availableHeight)
            {
                bestText = wrapped;
                bestSize = size;
                break;
            }

            bestText = wrapped;
            bestSize = size;
        }

        targetText.fontSize = bestSize;
        targetText.text = bestText;

        if (!disableAutoSize)
            targetText.enableAutoSizing = originalAutoSize;

        return bestText;
    }

    private string WrapTextByGroup(string rawText, float maxWidth)
    {
        if (string.IsNullOrEmpty(rawText))
            return "";

        string[] paragraphs = rawText.Replace("\r\n", "\n").Split('\n');
        StringBuilder result = new StringBuilder();

        for (int p = 0; p < paragraphs.Length; p++)
        {
            string paragraph = paragraphs[p];

            if (string.IsNullOrEmpty(paragraph))
            {
                result.AppendLine();
                continue;
            }

            List<string> tokens = TokenizeBySpace(paragraph);

            string currentLine = "";

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                string candidate = string.IsNullOrEmpty(currentLine)
                    ? token
                    : currentLine + " " + token;

                float candidateWidth = targetText.GetPreferredValues(candidate, Mathf.Infinity, Mathf.Infinity).x;

                if (candidateWidth <= maxWidth)
                {
                    currentLine = candidate;
                    continue;
                }

                // 현재 줄이 비어있지 않으면 먼저 확정
                if (!string.IsNullOrEmpty(currentLine))
                {
                    result.AppendLine(currentLine);
                    currentLine = "";
                }

                // 토큰 하나가 너무 길면 글자 단위로 강제 줄바꿈
                float tokenWidth = targetText.GetPreferredValues(token, Mathf.Infinity, Mathf.Infinity).x;
                if (tokenWidth > maxWidth)
                {
                    List<string> brokenParts = BreakLongToken(token, maxWidth);

                    for (int j = 0; j < brokenParts.Count; j++)
                    {
                        bool isLast = j == brokenParts.Count - 1;
                        if (isLast)
                            currentLine = brokenParts[j];
                        else
                            result.AppendLine(brokenParts[j]);
                    }
                }
                else
                {
                    currentLine = token;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
                result.Append(currentLine);

            if (p < paragraphs.Length - 1)
                result.AppendLine();
        }

        return result.ToString();
    }

    private List<string> TokenizeBySpace(string text)
    {
        List<string> tokens = new List<string>();
        string[] split = text.Split(' ');

        for (int i = 0; i < split.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(split[i]))
                tokens.Add(split[i]);
        }

        return tokens;
    }

    private List<string> BreakLongToken(string token, float maxWidth)
    {
        List<string> result = new List<string>();

        if (string.IsNullOrEmpty(token))
        {
            result.Add("");
            return result;
        }

        StringBuilder current = new StringBuilder();

        for (int i = 0; i < token.Length; i++)
        {
            string candidate = current.ToString() + token[i];
            float width = targetText.GetPreferredValues(candidate, Mathf.Infinity, Mathf.Infinity).x;

            if (width <= maxWidth)
            {
                current.Append(token[i]);
            }
            else
            {
                if (current.Length > 0)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }

                current.Append(token[i]);
            }
        }

        if (current.Length > 0)
            result.Add(current.ToString());

        return result;
    }
}