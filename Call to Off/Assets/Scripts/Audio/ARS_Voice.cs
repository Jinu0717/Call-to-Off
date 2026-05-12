using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class NodeVoiceClip
{
    [Header("ARS 노드 ID")]
    public int nodeId;

    [Header("이 노드에서 재생할 오디오")]
    public List<AudioClip> clips = new List<AudioClip>();
}

public class ARS_Voice : MonoBehaviour
{
    [Header("오디오 출력기")]
    [SerializeField] private AudioSource audioSource;

    [Header("노드별 음성 클립")]
    [Tooltip("노드 하나당 최소 1개의 오디오를 넣어주세요.")]
    [SerializeField] private List<NodeVoiceClip> nodeVoiceClips = new List<NodeVoiceClip>();

    [Header("숫자 음성 클립")]
    [Tooltip("0~9 숫자 오디오를 넣어주세요. 파일 이름은 0, 1, 2처럼 숫자로 끝나야 합니다.")]
    [SerializeField] private List<AudioClip> numberClips = new List<AudioClip>();

    [Header("공통 안내문 마지막 클립")]
    [SerializeField] private AudioClip lastClip;

    [Header("재생 간격")]
    [SerializeField] private float clipGap = 0.05f;

    [Header("{} 숫자 자동 음성 출력")]
    [SerializeField] private bool usePlaceholderNumberVoice = true;

    [Header("디버그")]
    [SerializeField] private bool useDebugLog = true;

    private readonly Dictionary<char, AudioClip> numberClipMap = new Dictionary<char, AudioClip>();

    private Coroutine playRoutine;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        PrepareAudioSource();
        BuildNumberClipMap();
    }

    private void PrepareAudioSource()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void BuildNumberClipMap()
    {
        numberClipMap.Clear();

        for (int i = 0; i < numberClips.Count; i++)
        {
            AudioClip clip = numberClips[i];

            if (clip == null)
                continue;

            if (!TryGetDigitFromClipName(clip.name, out char digit))
            {
                if (useDebugLog)
                    Debug.LogWarning($"[ARS_Voice] 숫자 오디오 이름을 해석하지 못했습니다: {clip.name}");

                continue;
            }

            if (!numberClipMap.ContainsKey(digit))
                numberClipMap.Add(digit, clip);
        }
    }

    private bool TryGetDigitFromClipName(string clipName, out char digit)
    {
        digit = '\0';

        if (string.IsNullOrWhiteSpace(clipName))
            return false;

        /*
         * 허용 예시:
         * 0
         * 1
         * 2
         * Number_0
         * Digit_3
         */
        Match match = Regex.Match(clipName.Trim(), @"([0-9])$");

        if (!match.Success)
            return false;

        digit = match.Groups[1].Value[0];
        return true;
    }

    public void PlayNode(ARSNodeData node, ARSTreeData treeData, bool playLastClip)
    {
        if (node == null)
        {
            StopVoice();
            return;
        }

        PrepareAudioSource();

        if (numberClipMap.Count == 0)
            BuildNumberClipMap();

        StopVoice();

        NodeVoiceClip target = FindNodeVoiceClip(node.nodeId);

        if (target == null)
        {
            if (useDebugLog)
                Debug.LogWarning($"[ARS_Voice] nodeId {node.nodeId}에 등록된 NodeVoiceClip이 없습니다.");

            return;
        }

        if (target.clips == null || target.clips.Count == 0)
        {
            if (useDebugLog)
                Debug.LogWarning($"[ARS_Voice] nodeId {node.nodeId}에 오디오가 하나도 없습니다.");

            return;
        }

        playRoutine = StartCoroutine(CoPlayNodeVoice(node, treeData, target.clips, playLastClip));
    }

    private NodeVoiceClip FindNodeVoiceClip(int nodeId)
    {
        for (int i = 0; i < nodeVoiceClips.Count; i++)
        {
            if (nodeVoiceClips[i] != null && nodeVoiceClips[i].nodeId == nodeId)
                return nodeVoiceClips[i];
        }

        return null;
    }

    public void StopVoice()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        if (audioSource != null)
            audioSource.Stop();

        IsPlaying = false;
    }

    private IEnumerator CoPlayNodeVoice(
    ARSNodeData node,
    ARSTreeData treeData,
    List<AudioClip> clips,
    bool playLastClip
)
    {
        IsPlaying = true;

        List<string> numberGroups = new List<string>();

        if (usePlaceholderNumberVoice && treeData != null)
            numberGroups = ExtractPlaceholderNumbers(node.dialogue, treeData);

        if (numberGroups.Count > 0)
        {
            yield return CoPlayClipsWithNumbers(clips, numberGroups);
        }
        else
        {
            yield return CoPlayClipsOnly(clips);
        }

        if (playLastClip && lastClip != null)
        {
            yield return CoPlayClip(lastClip);
        }

        IsPlaying = false;
        playRoutine = null;
    }

    private List<string> ExtractPlaceholderNumbers(string rawDialogue, ARSTreeData treeData)
    {
        List<string> result = new List<string>();

        if (string.IsNullOrEmpty(rawDialogue))
            return result;

        /*
         * 대사 안에서 {PHONE_FULL}, {LIGHT_DEVICE_NO} 같은 부분을 찾는다.
         */
        MatchCollection matches = Regex.Matches(rawDialogue, @"\{[A-Za-z0-9_]+\}");

        for (int i = 0; i < matches.Count; i++)
        {
            string placeholder = matches[i].Value;

            /*
             * ARSTreeData의 FormatDialogue를 이용해서
             * {PHONE_FULL} 같은 값을 실제 숫자 문자열로 바꾼다.
             */
            string formatted = treeData.FormatDialogue(placeholder);

            /*
             * 알 수 없는 placeholder라면 그대로 반환될 가능성이 있으므로 제외한다.
             */
            if (formatted == placeholder)
                continue;

            string digits = ExtractDigitsOnly(formatted);

            if (!string.IsNullOrEmpty(digits))
                result.Add(digits);
        }

        return result;
    }

    private string ExtractDigitsOnly(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        string digits = "";

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
                digits += text[i];
        }

        return digits;
    }

    private IEnumerator CoPlayClipsWithNumbers(List<AudioClip> clips, List<string> numberGroups)
    {
        /*
         * 예시 1:
         *
         * 대사:
         * "고객님의 전화번호는 {PHONE_FULL} 입니다."
         *
         * Clips:
         * 0번: "고객님의 전화번호는"
         * 1번: "입니다."
         *
         * 재생:
         * clips[0]
         * 숫자 음성
         * clips[1]
         *
         *
         * 예시 2:
         *
         * 대사:
         * "기기 번호는 {DEVICE_NO}, 상세 코드는 {DETAIL_CODE} 입니다."
         *
         * Clips:
         * 0번: "기기 번호는"
         * 1번: "상세 코드는"
         * 2번: "입니다."
         *
         * 재생:
         * clips[0]
         * 숫자 음성
         * clips[1]
         * 숫자 음성
         * clips[2]
         */

        int clipIndex = 0;

        for (int i = 0; i < numberGroups.Count; i++)
        {
            if (clipIndex < clips.Count)
            {
                yield return CoPlayClip(clips[clipIndex]);
                clipIndex++;
            }

            yield return CoPlayDigits(numberGroups[i]);
        }

        while (clipIndex < clips.Count)
        {
            yield return CoPlayClip(clips[clipIndex]);
            clipIndex++;
        }
    }

    private IEnumerator CoPlayClipsOnly(List<AudioClip> clips)
    {
        /*
         * {} 숫자 삽입이 없는 일반 노드는
         * 등록된 오디오를 순서대로 재생한다.
         *
         * clips가 1개면 1개만 재생.
         * clips가 2개 이상이면 순서대로 재생.
         */

        for (int i = 0; i < clips.Count; i++)
        {
            yield return CoPlayClip(clips[i]);
        }
    }

    private IEnumerator CoPlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            yield break;

        audioSource.clip = clip;
        audioSource.Play();

        while (audioSource != null && audioSource.isPlaying)
            yield return null;

        if (clipGap > 0f)
            yield return new WaitForSeconds(clipGap);
    }

    private IEnumerator CoPlayDigits(string digits)
    {
        if (string.IsNullOrEmpty(digits))
            yield break;

        for (int i = 0; i < digits.Length; i++)
        {
            char digit = digits[i];

            if (!numberClipMap.TryGetValue(digit, out AudioClip digitClip))
            {
                if (useDebugLog)
                    Debug.LogWarning($"[ARS_Voice] 숫자 {digit}에 해당하는 오디오가 없습니다.");

                continue;
            }

            yield return CoPlayClip(digitClip);
        }
    }

    public float GetNodeVoiceLength(ARSNodeData node, ARSTreeData treeData, bool playLastClip)
    {
        if (node == null)
            return 0f;

        if (numberClipMap.Count == 0)
            BuildNumberClipMap();

        NodeVoiceClip target = FindNodeVoiceClip(node.nodeId);

        if (target == null || target.clips == null || target.clips.Count == 0)
            return 0f;

        float totalLength = 0f;

        List<string> numberGroups = new List<string>();

        if (usePlaceholderNumberVoice && treeData != null)
            numberGroups = ExtractPlaceholderNumbers(node.dialogue, treeData);

        if (numberGroups.Count > 0)
        {
            totalLength += GetClipsWithNumbersLength(target.clips, numberGroups);
        }
        else
        {
            totalLength += GetClipsOnlyLength(target.clips);
        }

        if (playLastClip && lastClip != null)
        {
            totalLength += lastClip.length;
            totalLength += clipGap;
        }

        return totalLength;
    }

    private float GetClipsWithNumbersLength(List<AudioClip> clips, List<string> numberGroups)
    {
        float totalLength = 0f;
        int clipIndex = 0;

        for (int i = 0; i < numberGroups.Count; i++)
        {
            if (clipIndex < clips.Count)
            {
                totalLength += GetSingleClipLength(clips[clipIndex]);
                clipIndex++;
            }

            totalLength += GetDigitsLength(numberGroups[i]);
        }

        while (clipIndex < clips.Count)
        {
            totalLength += GetSingleClipLength(clips[clipIndex]);
            clipIndex++;
        }

        return totalLength;
    }

    private float GetClipsOnlyLength(List<AudioClip> clips)
    {
        float totalLength = 0f;

        for (int i = 0; i < clips.Count; i++)
        {
            totalLength += GetSingleClipLength(clips[i]);
        }

        return totalLength;
    }

    private float GetDigitsLength(string digits)
    {
        if (string.IsNullOrEmpty(digits))
            return 0f;

        float totalLength = 0f;

        for (int i = 0; i < digits.Length; i++)
        {
            char digit = digits[i];

            if (!numberClipMap.TryGetValue(digit, out AudioClip digitClip))
                continue;

            totalLength += GetSingleClipLength(digitClip);
        }

        return totalLength;
    }

    private float GetSingleClipLength(AudioClip clip)
    {
        if (clip == null)
            return 0f;

        return clip.length + clipGap;
    }
}