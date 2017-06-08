using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Other/Dithering Simple")]
public class DitheringEffectSimple : MonoBehaviour
{
    public Material material;
    public int ColorCount = 4;
	public int PaletteHeight = 64;
	public Texture PaletteTexture;

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_ColorCount", ColorCount);
		material.SetFloat("_PaletteHeight", PaletteHeight);
		material.SetTexture("_PaletteTex", PaletteTexture);
		Graphics.Blit(source, destination, material);
	}
}