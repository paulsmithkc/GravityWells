using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinShake : MonoBehaviour
{

    public float duration = 0.5f;
    public float speed = 1.0f;
    public float magnitude = 1.0f;

    private bool isShaking = false;

    public void Shake()
    {
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

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            Vector3 pos = Camera.main.transform.position;

            float alpha = randomStart + speed * percentComplete;

            float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
            float y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

            x *= magnitude * damper;
            y *= magnitude * damper;

            Vector3 shake = Camera.main.transform.right * x + Camera.main.transform.up * y;
            Camera.main.transform.position = pos + shake;

            yield return null;
        }

        isShaking = false;
    }
}
