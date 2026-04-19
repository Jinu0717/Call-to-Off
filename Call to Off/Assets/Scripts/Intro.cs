using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Animator cellPhoneAnim;
    [SerializeField] 
    private TMP_TypeByAnimationLength typeWriter;
    [SerializeField]
    [TextArea(2, 5)]
    private string[] dialogues;

    private int currentIndex = 1;
    private bool isEnd = false;

    private void Awake() => typeWriter.Play(dialogues[0]);

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }

        if (isEnd) { return; }

        if (typeWriter.IsTyping)
        {
            typeWriter.Skip();
            return;
        }

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

        if (currentIndex >= dialogues.Length)
        {
            isEnd = true;
            cellPhoneAnim.enabled = true;
            typeWriter.Play("");
            Debug.Log("모든 대사 출력 완료");
            return;
        }

        playerAnim.SetTrigger("Click");
        typeWriter.Play(dialogues[currentIndex]);

        currentIndex++;
    }
}