using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Fail : MonoBehaviour
{
    [SerializeField]
    private GameObject intro;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private GameObject ARS;
    [SerializeField]
    private GameObject fail;

    [SerializeField]
    [TextArea(2, 5)]
    private string[] dialogues;

    [SerializeField]
    private TMP_TypeByAnimationLength typeWriter;
    [SerializeField]
    private TMPSmartWrappedLayout smartLayout;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private int nextNum;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private SpriteRenderer[] customersSR;
    [SerializeField]
    private Sprite customerSprite;

    [Header("실패 카메라 흔들림")]
    [SerializeField]
    private CameraShaking camShake;

    [SerializeField]
    private float shakeInterval = 0.12f;

    [SerializeField]
    private float shakeDuration = 0.12f;

    [SerializeField]
    private float startShakePower = 0.03f;

    [SerializeField]
    private float shakePowerPerDialogue = 0.025f;

    [SerializeField]
    private float maxShakePower = 0.25f;

    private int currentIndex = 0;
    private bool isPlaying = false;
    private bool isEnd = false;

    private Coroutine warningSoundRoutine;
    private Coroutine failShakeRoutine;

    private float currentShakePower;

    public void Finish()
    {
        Debug.Log("Fail");

        for (int i = 0; i < customersSR.Length; i++) { customersSR[i].sprite = customerSprite; }

        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        intro.SetActive(false);
        hand.SetActive(false);
        ARS.SetActive(false);

        fail.SetActive(true);

        StartOutro();

        if (warningSoundRoutine != null)
            StopCoroutine(warningSoundRoutine);

        warningSoundRoutine = StartCoroutine(AudioSetting());

        StartFailShake();
    }

    private void StartOutro()
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("[Fail] dialogues 비어 있음");
            return;
        }

        currentIndex = 0;
        isPlaying = true;
        isEnd = false;

        UpdateShakePower();

        PlayDialogue(dialogues[currentIndex]);
    }

    private void Update()
    {
        if (!isPlaying) return;
        if (isEnd) return;

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        // 타이핑 중이면 무시
        if (typeWriter != null && typeWriter.IsTyping)
            return;

        ShowNextDialogue();
    }

    private void ShowNextDialogue()
    {
        currentIndex++;

        UpdateShakePower();

        if (currentIndex == nextNum)
        {
            if (anim != null)
                anim.SetTrigger("Click");
        }

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

        StopFailShake();

        if (warningSoundRoutine != null)
        {
            StopCoroutine(warningSoundRoutine);
            warningSoundRoutine = null;
        }

        Debug.Log("[Fail] 모든 대사 출력 완료");

        Invoke(nameof(Loading), 0.5f);
    }

    private void Loading() => SceneManager.LoadScene("Fail");

    private IEnumerator AudioSetting()
    {
        SFXManager sfx = FindAnyObjectByType<SFXManager>();

        while (currentIndex < nextNum && !isEnd)
        {
            if (sfx != null)
                sfx.warningSound();

            yield return new WaitForSeconds(1f);
        }

        warningSoundRoutine = null;
    }

    private void StartFailShake()
    {
        StopFailShake();

        UpdateShakePower();
        failShakeRoutine = StartCoroutine(CoFailShake());
    }

    private void StopFailShake()
    {
        if (failShakeRoutine != null)
        {
            StopCoroutine(failShakeRoutine);
            failShakeRoutine = null;
        }
    }

    private IEnumerator CoFailShake()
    {
        while (isPlaying && !isEnd)
        {
            if (camShake != null)
                camShake.Shake(shakeDuration, currentShakePower, false);

            yield return new WaitForSeconds(shakeInterval);
        }

        failShakeRoutine = null;
    }

    private void UpdateShakePower()
    {
        currentShakePower = startShakePower + (currentIndex * shakePowerPerDialogue);
        currentShakePower = Mathf.Min(currentShakePower, maxShakePower);
    }
}