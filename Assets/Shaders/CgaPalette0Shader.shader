Shader "Custom/CGA Palette 0" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColorCount ("Mixed Color Count", float) = 4
		_PaletteHeight ("Palette Height", float) = 128
		_PaletteTex ("Palette", 2D) = "black" {}
		_DitherSize ("Dither Size (Width/Height)", float) = 8
		_DitherTex ("Dither", 2D) = "black" {}
	}

	SubShader {
		Tags { "IgnoreProjector"="True" "RenderType"="Opaque" }
		LOD 110

		Lighting Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _PaletteTex;
			sampler2D _DitherTex;
			float _ColorCount;
			float _PaletteHeight;
			float _DitherSize;

			struct VertexInput {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct FragmentInput {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 ditherPos : TEXCOORD1;
			};

			inline float4 GetDitherPos(float4 vertex, float ditherSize) {
				// Get the dither pixel position from the screen coordinates.
				float4 screenPos = ComputeScreenPos(UnityObjectToClipPos(vertex));
				return float4(screenPos.xy * _ScreenParams.xy / ditherSize, 0, screenPos.w);
			}

			FragmentInput vert(VertexInput i) {
				FragmentInput o;
				o.position = UnityObjectToClipPos(i.position);
				o.uv = i.uv;
				o.ditherPos = GetDitherPos(i.position, _DitherSize);
				return o;
			}

			inline fixed3 GetDitherColor(fixed3 color, sampler2D ditherTex, float4 ditherPos) {
				
				float ditherValue = tex2D(ditherTex, ditherPos.xy / ditherPos.w).r;
				return fixed3(
					step(0.5, floor(color.r * 16) / 16 - 0.1 * ditherValue), 
					step(0.5, floor(color.g * 16) / 16 - 0.1 * ditherValue),
					0
				);
			}

			fixed4 frag(FragmentInput i) : COLOR {
				fixed4 c = tex2D(_MainTex, i.uv);
				return fixed4(GetDitherColor(c.rgb, _DitherTex, i.ditherPos), c.a);
			}
			ENDCG
		}
	}

	Fallback "Unlit/Texture"
}