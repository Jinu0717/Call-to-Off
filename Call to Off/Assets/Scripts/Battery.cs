using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private float presentTime;
    [SerializeField] private float divTime;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

    [Header("배터리 종료 시 실행할 Fail")]
    [SerializeField] private Fail fail;

    private bool isFinished = false;

    private void Awake()
    {
        presentTime = 0f;

        if (sprites != null && sprites.Length > 1)
            divTime = maxTime / (sprites.Length - 1);
    }

    private void Update()
    {
        if (isFinished)
            return;

        presentTime += Time.deltaTime;

        if (presentTime >= maxTime)
        {
            presentTime = maxTime;

            if (image != null && sprites != null && sprites.Length > 0)
                image.sprite = sprites[sprites.Length - 1];

            FinishBattery();
            return;
        }

        UpdateBatteryImage();
    }

    private void UpdateBatteryImage()
    {
        if (image == null)
            return;

        if (sprites == null || sprites.Length == 0)
            return;

        if (divTime <= 0f)
            return;

        int num = (int)(presentTime / divTime);
        num = Mathf.Clamp(num, 0, sprites.Length - 1);

        image.sprite = sprites[num];
    }

    private void FinishBattery()
    {
        if (isFinished)
            return;

        isFinished = true;

        if (fail != null)
        {
            fail.Finish();
        }
        else
        {
            Debug.LogWarning("[Battery] Fail에 연결되지 않았습니다.");
        }

        enabled = false;
    }
}