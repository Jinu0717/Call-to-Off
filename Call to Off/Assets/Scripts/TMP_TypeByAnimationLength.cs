using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMP_TypeByAnimationLength : MonoBehaviour
{
    public enum TypingMode
    {
        UseAnimationLength,
        UseFixedCharacterTime
    }

    [SerializeField] private TMP_Text targetText;

    [Header("Animator 연결")]
    [SerializeField] private Animator animator;

    [Header("출력 옵션")]
    [SerializeField] private TypingMode typingMode = TypingMode.UseAnimationLength;

    [Tooltip("UseFixedCharacterTime일 때 완성 글자 1개당 걸리는 시간")]
    [SerializeField] private float timePerCharacter = 0.1f;

    public bool IsTyping { get; private set; }
    public bool IsFinished { get; private set; }
    public TypingMode CurrentMode => typingMode;

    private string currentFullText = "";
    private Coroutine typingCoroutine;

    private static readonly string[] CHOSEONG =
    {
        "ㄱ","ㄲ","ㄴ","ㄷ","ㄸ","ㄹ","ㅁ","ㅂ","ㅃ","ㅅ",
        "ㅆ","ㅇ","ㅈ","ㅉ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ"
    };

    private const int HANGUL_BASE = 0xAC00;
    private const int HANGUL_LAST = 0xD7A3;
    private const int V_COUNT = 21;
    private const int T_COUNT = 28;
    private const int N_COUNT = V_COUNT * T_COUNT;

    private void Reset()
    {
        targetText = GetComponent<TMP_Text>();
    }

    public void StopTypingRoutineOnly()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        IsTyping = false;
    }

    public void ClearText()
    {
        StopTypingRoutineOnly();

        currentFullText = "";

        if (targetText != null)
            targetText.text = "";

        IsFinished = false;
    }

    public void Play(string text)
    {
        if (targetText == null)
        {
            Debug.LogWarning("[TMP_TypeByAnimationLength] TMP_Text가 연결되지 않았습니다.");
            return;
        }

        StopTypingRoutineOnly();

        currentFullText = text ?? "";
        typingCoroutine = StartCoroutine(TypeRoutine(currentFullText));
    }

    private IEnumerator TypeRoutine(string text)
    {
        IsTyping = true;
        IsFinished = false;
        targetText.text = "";

        yield return null;

        if (string.IsNullOrEmpty(text))
        {
            targetText.text = text;
            IsTyping = false;
            IsFinished = true;
            typingCoroutine = null;
            yield break;
        }

        if (typingMode == TypingMode.UseAnimationLength)
            yield return StartCoroutine(TypeByAnimationLength(text));
        else
            yield return StartCoroutine(TypeByFixedCharacterTime(text));

        targetText.text = text;
        IsTyping = false;
        IsFinished = true;
        typingCoroutine = null;
    }

    private IEnumerator TypeByAnimationLength(string text)
    {
        List<string> frames = BuildTypingFrames(text);

        if (frames.Count == 0)
            yield break;

        float animLength = GetAnimationLength();

        if (animLength <= 0f)
        {
            targetText.text = text;
            yield break;
        }

        float stepTime = animLength / frames.Count;

        for (int i = 0; i < frames.Count; i++)
        {
            targetText.text = frames[i];

            if (i < frames.Count - 1)
                yield return new WaitForSeconds(stepTime);
        }
    }

    private IEnumerator TypeByFixedCharacterTime(string text)
    {
        string fixedPart = "";

        foreach (char c in text)
        {
            List<string> steps = GetCharacterSteps(c);

            if (steps == null || steps.Count == 0)
            {
                fixedPart += c;
                targetText.text = fixedPart;
                continue;
            }

            float totalCharTime = Mathf.Max(0f, timePerCharacter);
            float stepTime = totalCharTime / steps.Count;

            for (int i = 0; i < steps.Count; i++)
            {
                targetText.text = fixedPart + steps[i];

                if (i < steps.Count - 1)
                    yield return new WaitForSeconds(stepTime);
            }

            fixedPart += c;
            yield return new WaitForSeconds(stepTime);
        }
    }

    private float GetAnimationLength()
    {
        if (animator == null)
            return 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }

    private List<string> BuildTypingFrames(string text)
    {
        List<string> frames = new List<string>();
        string fixedPart = "";

        foreach (char c in text)
        {
            List<string> steps = GetCharacterSteps(c);

            for (int i = 0; i < steps.Count; i++)
                frames.Add(fixedPart + steps[i]);

            fixedPart += c;
        }

        return frames;
    }

    private List<string> GetCharacterSteps(char c)
    {
        List<string> steps = new List<string>();

        if (char.IsWhiteSpace(c))
        {
            steps.Add(c.ToString());
            return steps;
        }

        if (IsHangulSyllable(c))
        {
            DecomposeHangul(c, out int lIndex, out int vIndex, out int tIndex);

            steps.Add(CHOSEONG[lIndex]);

            char lvChar = ComposeLV(lIndex, vIndex);
            steps.Add(lvChar.ToString());

            if (tIndex > 0)
                steps.Add(c.ToString());

            return steps;
        }

        steps.Add(c.ToString());
        return steps;
    }

    private bool IsHangulSyllable(char c)
    {
        return c >= HANGUL_BASE && c <= HANGUL_LAST;
    }

    private void DecomposeHangul(char syllable, out int lIndex, out int vIndex, out int tIndex)
    {
        int sIndex = syllable - HANGUL_BASE;
        lIndex = sIndex / N_COUNT;
        vIndex = (sIndex % N_COUNT) / T_COUNT;
        tIndex = sIndex % T_COUNT;
    }

    private char ComposeLV(int lIndex, int vIndex)
    {
        int code = HANGUL_BASE + ((lIndex * V_COUNT) + vIndex) * T_COUNT;
        return (char)code;
    }
}