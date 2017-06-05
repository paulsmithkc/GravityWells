using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelateImageEffect : MonoBehaviour {
    
    [Range(0, 7)]
    public int downsample = 1;
    
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture t = RenderTexture.GetTemporary(src.width >> downsample, src.height >> downsample);
        Graphics.Blit(src, t);
        Graphics.Blit(t, dest);
        RenderTexture.ReleaseTemporary(t);
    }
}
