using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Other/Dithering")]
public class DitheringEffect : MonoBehaviour
{
    public Material material;
	public int ColorCount = 4;
	public int PaletteHeight = 64;
	public Texture PaletteTexture;
	public int DitherSize = 8;
	public Texture DitherTexture;
	
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_ColorCount", ColorCount);
		material.SetFloat("_PaletteHeight", PaletteHeight);
		material.SetTexture("_PaletteTex", PaletteTexture);
		material.SetFloat("_DitherSize", DitherSize);
		material.SetTexture("_DitherTex", DitherTexture);
		Graphics.Blit(source, destination, material);
	}
}