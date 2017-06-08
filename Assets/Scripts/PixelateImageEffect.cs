using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelateImageEffect : MonoBehaviour {
    
    //[Range(0, 1024)]
    public int downsample = 1;
    
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (downsample <= 1)
        {
            Graphics.Blit(src, dest);
            return;
        }

        RenderTexture t = RenderTexture.GetTemporary(
            src.width / downsample, 
            src.height / downsample
        );
        src.filterMode = FilterMode.Point;
        dest.filterMode = FilterMode.Point;
        t.filterMode = FilterMode.Point;

        Graphics.Blit(src, t);
        Graphics.Blit(t, dest);
        RenderTexture.ReleaseTemporary(t);
    }
}
