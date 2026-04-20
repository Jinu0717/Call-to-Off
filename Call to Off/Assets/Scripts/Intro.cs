using System.Collections;
using TMPro;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private GameObject ARSText;
    [SerializeField]
    private GameObject[] canvases;
    [SerializeField]
    private SpriteRenderer phoneSR;
    [SerializeField]
    private Sprite numberSprite;
    [SerializeField]
    private RectTransform barWidth;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Animator cellPhoneAnim;
    [SerializeField]
    private SpriteRenderer[] peaple;
    [SerializeField]
    private Sprite[] personSprite;
    [SerializeField]
    private TMP_TypeByAnimationLength typeWriter;
    [SerializeField]
    [TextArea(2, 5)]
    private string[] dialogues;
    [SerializeField]
    private CameraShaking camShake;
    [SerializeField]
    private SpriteRenderer playerSR;
    private const string TARGET_SPRITE_NAME = "Player Sit 4";

    private int currentIndex = 1;
    private bool isEnd = false;
    private Coroutine playerMotionCheckRoutine;

    private void Awake()
    {
        hand.SetActive(false);
        ARSText.SetActive(false);

        canvases[0].SetActive(true);
        canvases[1].SetActive(false);

        cellPhoneAnim.enabled = false;

        isEnd = false;
    }

    private void Start()
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("[Intro] dialogues가 비어 있습니다.");
            isEnd = true;
            return;
        }

        typeWriter.StopTypingRoutineOnly();
        typeWriter.Play(dialogues[0]);
        UpdateBarWidth();
        ApplyPeopleSprite(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (isEnd)
            return;

        // 출력 코루틴 진행 중이면 입력 무시
        if (typeWriter != null && typeWriter.IsTyping)
            return;

        ShowNextDialogue();
    }

    private void ShowNextDialogue()
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("[Intro] dialogues가 비어 있습니다.");
            isEnd = true;
            return;
        }

        int len = dialogues.Length;

        if (currentIndex >= len)
        {
            FinishIntro();
            return;
        }

        if (typeWriter != null)
            typeWriter.StopTypingRoutineOnly();

        HandleCellPhoneEventByIndex(currentIndex);
        UpdateBarWidth();

        playerAnim.SetTrigger("Click");
        StartPlayerMotionCheck();

        typeWriter.Play(dialogues[currentIndex]);
        currentIndex++;
    }

    private void HandleCellPhoneEventByIndex(int index)
    {
        if (index == 6)
        {
            cellPhoneAnim.enabled = true;
        }
        else if (index == 8 || index == 15 || index == 17)
        {
            cellPhoneAnim.SetTrigger("Click");
        }
    }

    private void UpdateBarWidth()
    {
        if (barWidth == null)
            return;

        float width = cellPhoneAnim != null && cellPhoneAnim.enabled ? 965f : 1821f;
        barWidth.sizeDelta = new Vector2(width, barWidth.sizeDelta.y);
    }

    private void FinishIntro()
    {
        if (typeWriter != null)
            typeWriter.StopTypingRoutineOnly();

        if (cellPhoneAnim != null && cellPhoneAnim.enabled)
            cellPhoneAnim.SetTrigger("Click");

        canvases[0].SetActive(false);
        canvases[1].SetActive(true);

        phoneSR.sprite = numberSprite;

        hand.SetActive(true);
        ARSText.SetActive(true);

        isEnd = true;
        typeWriter.Play("");
        Debug.Log("모든 대사 출력 완료");
    }

    private void StartPlayerMotionCheck()
    {
        if (playerMotionCheckRoutine != null)
            StopCoroutine(playerMotionCheckRoutine);

        playerMotionCheckRoutine = StartCoroutine(CoCheckPlayerMotion());
    }

    private IEnumerator CoCheckPlayerMotion()
    {
        float timeout = 1.0f;
        float elapsed = 0f;

        string prevName = playerSR.sprite.name;

        while (elapsed < timeout)
        {
            string currentName = playerSR.sprite.name;

            bool wasSit = prevName == TARGET_SPRITE_NAME;
            bool isSit = currentName == TARGET_SPRITE_NAME;

            if (!wasSit && isSit)
            {
                ApplyPeopleSprite(true);

                if (camShake != null)
                    camShake.Shake(0.12f, 0.08f);

                playerMotionCheckRoutine = null;
                yield break;
            }

            prevName = currentName;
            elapsed += Time.deltaTime;
            yield return null;
        }

        ApplyPeopleSprite(false);
        playerMotionCheckRoutine = null;
    }

    private void ApplyPeopleSprite(bool isSit)
    {
        if (peaple == null || peaple.Length == 0)
            return;
        if (personSprite == null || personSprite.Length < 2)
            return;

        int index = isSit ? 1 : 0;

        for (int i = 0; i < peaple.Length; i++)
        {
            if (peaple[i] != null)
                peaple[i].sprite = personSprite[index];
        }
    }
}