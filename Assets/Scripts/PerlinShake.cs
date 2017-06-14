using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinShake : MonoBehaviour
{

    public float duration = 0.5f;
    public float speed = 1.0f;
    public float magnitude = 1.0f;
    public float multiplier = 1.0f;

    private bool isShaking = false;

    public void Shake(float multiplier)
    {
        this.multiplier = multiplier;
        if (!isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    private IEnumerator DoShake()
    {
        isShaking = true;
        float elapsed = 0.0f;

        float randomStart = Random.Range(-1000.0f, 1000.0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            Transform cameraTransform = this.transform;
            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            Vector3 pos = cameraTransform.position;

            float alpha = randomStart + speed * percentComplete;

            float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
            float y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

            float m = Mathf.Clamp(multiplier * magnitude * damper, 0.0f, 0.5f);
            Debug.Log(m);
            x *= m;
            y *= m;

            Vector3 shake = cameraTransform.right * x + cameraTransform.up * y;
            cameraTransform.position = pos + shake;

            yield return new WaitForEndOfFrame();
        }

        isShaking = false;
    }
}
