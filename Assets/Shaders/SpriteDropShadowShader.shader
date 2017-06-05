Shader "Custom/Sprite Drop Shadow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _DropShadowColor("Color", Color) = (0,0,0,1)
        _DropShadowOffset("Drop Shadow Offset", Range(0,1)) = 0.1
        _DropShadowScale("Drop Shadow Scale", Range(0,10)) = 1.1
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

        Pass
		{
            Name "DROP_SHADOW"
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
                float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            uniform fixed4 _DropShadowColor;
            uniform float _DropShadowOffset;
            uniform float _DropShadowScale;
			
			v2f vert (appdata v)
			{
                v.vertex = (v.vertex + v.normal * _DropShadowOffset) * _DropShadowScale;
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex.x += _DropShadowOffset;
                //o.vertex.y += _DropShadowOffset;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = _DropShadowColor.rgb;
                col.a *= _DropShadowColor.a;
				return col;
			}
			ENDCG
		}
		Pass
		{
            Name "FORWARD"
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
