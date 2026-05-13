using System.Collections;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    private Vector3 originalLocalPos;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        originalLocalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude, bool sound)
    {
        if (sound)
        {
            SFXManager sfx = FindAnyObjectByType<SFXManager>();
            sfx.thudSound();
        }

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 점점 약해지게
            float power = 1f - (elapsed / duration);

            float x = Random.Range(-1f, 1f) * magnitude * power;
            float y = Random.Range(-1f, 1f) * magnitude * power;

            transform.localPosition = originalLocalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeCoroutine = null;
    }
}