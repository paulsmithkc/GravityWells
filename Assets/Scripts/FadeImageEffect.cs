using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class FadeImageEffect : MonoBehaviour {

    public Material material;
    public Color startColor = Color.black;
    public Color endColor = Color.black;
    public Color currentColor = Color.black;
    public float timeElapsed = 0;
    public float timeToFade = 1;

    public void Fade(Color startColor, Color endColor, float time)
    {
        this.startColor = startColor;
        this.endColor = endColor;
        timeElapsed = 0;
        timeToFade = time;
    }

    void Start()
    {
        currentColor = startColor;
    }

    void Update()
    {
        timeElapsed = Mathf.Clamp(timeElapsed + Time.deltaTime, 0, timeToFade);
        currentColor = Color.Lerp(startColor, endColor, timeElapsed / timeToFade);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetColor("_Color", currentColor);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
