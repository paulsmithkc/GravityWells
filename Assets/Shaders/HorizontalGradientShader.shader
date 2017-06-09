Shader "Custom/Gradient Horizontal"
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Center Color", Color) = (1,1,1,1)
		_EdgeColor ("Edge Color", Color) = (1,1,1,0)
		_EdgeMultiplier ("Edge Multiplier", float) = 10
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off
            Cull Off
            ZWrite Off
            ZTest On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float4 _EdgeColor;
			uniform float _EdgeMultiplier;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float fade = saturate(abs(i.uv.y - 0.5) * 2 * _EdgeMultiplier);
				fixed4 col = lerp(_Color, _EdgeColor, fade);
				//col *= tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
