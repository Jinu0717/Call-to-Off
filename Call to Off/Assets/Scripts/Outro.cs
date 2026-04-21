using UnityEngine;

public class Outro : MonoBehaviour
{
    [SerializeField]
    private GameObject intro;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private GameObject ARS;
    [SerializeField]
    private GameObject outro;
    [SerializeField]
    [TextArea(2, 5)]
    private string[] dialogues;
    [SerializeField]
    private TMP_TypeByAnimationLength typeWriter;
    [SerializeField]
    private TMPSmartWrappedLayout smartLayout;
    private int currentIndex = 0;
    private bool isPlaying = false;
    private bool isEnd = false;

    public void Finish()
    {
        Debug.Log("Finsih");

        intro.SetActive(false);
        hand.SetActive(false);
        ARS.SetActive(false);

        outro.SetActive(true);

        StartOutro();
    }

    private void StartOutro()
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("[Outro] dialogues 비어 있음");
            return;
        }

        currentIndex = 0;
        isPlaying = true;
        isEnd = false;

        PlayDialogue(dialogues[currentIndex]);
    }

    private void Update()
    {
        if (!isPlaying) return;
        if (isEnd) return;

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        // 타이핑 중이면 무시 (지금 시스템 정책)
        if (typeWriter != null && typeWriter.IsTyping)
            return;

        ShowNextDialogue();
    }

    private void ShowNextDialogue()
    {
        currentIndex++;

        if (currentIndex >= dialogues.Length)
        {
            EndOutro();
            return;
        }

        PlayDialogue(dialogues[currentIndex]);
    }

    private void PlayDialogue(string text)
    {
        if (typeWriter == null) return;

        typeWriter.StopTypingRoutineOnly();

        // 스마트 줄바꿈 적용
        if (smartLayout != null && smartLayout.TargetText == typeWriter.TargetText)
        {
            string wrapped = smartLayout.ApplyBestLayout(text);
            typeWriter.Play(wrapped);
        }
        else
        {
            typeWriter.Play(text);
        }
    }

    private void EndOutro()
    {
        isEnd = true;
        isPlaying = false;

        Debug.Log("[Outro] 모든 대사 출력 완료");

        // 여기서 추가 연출 가능 (페이드아웃 등)
    }
}
