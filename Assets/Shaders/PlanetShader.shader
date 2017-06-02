Shader "Custom/Planet" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        _PhongTess ("Phong Tessellation", Range(0,1)) = 0.5
        _EdgeTess ("Edge Length Tessellation", Range(2,50)) = 5

        [Space(10)][Header(Rim lighting)]
        _RimPower ("Rim Power", Range(0,9)) = 3
        _RimMultiplier ("Rim Multiplier", Range(0,5)) = 2
        _RimColor("Rim Color", Color) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
        #include "Tessellation.cginc"
		#pragma surface surf Standard fullforwardshadows tessellate:tess tessphong:_PhongTess
		#pragma target 5.0

		sampler2D _MainTex;

        uniform half _Glossiness;
        uniform half _Metallic;
        uniform half _PhongTess;
        uniform half _EdgeTess;
        uniform half _RimPower;
        uniform half _RimMultiplier;
        uniform fixed4 _Color;
        uniform fixed4 _RimColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
		    // put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
        };

        float4 tess(appdata_full v0, appdata_full v1, appdata_full v2) {
            return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeTess);
        }

        struct Input {
            float2 uv_MainTex;
            float3 viewDir;
        };

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			
            float interior = saturate(dot(normalize(IN.viewDir), o.Normal));
            float rimStrength = _RimMultiplier * saturate(pow(1.0 - interior, _RimPower));
            c = lerp(c, _RimColor, rimStrength);

            o.Albedo = c.rgb;
            o.Alpha = 1.0;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
