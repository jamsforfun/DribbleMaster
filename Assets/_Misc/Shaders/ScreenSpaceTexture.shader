Shader "Custom/ScreenSpaceTexture" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_ScreenTex ("Albedo (RGB)", 2D) = "white" {}
		_FadeX ("Fade X", float) = 1
		_FadeY ("Fade Y", float) = 1
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200
   
        CGPROGRAM
 
        #pragma surface surf Standard noforwardadd alpha:fade
        #pragma target 3.0

		sampler2D _ScreenTex;
		float _FadeX, _FadeY;
		float3 _Color;

		struct Input {
			float2 uv_ScreenTex;
			float4 screenPos;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			half2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			fixed4 sstc = tex2D(_ScreenTex, screenUV);
			o.Albedo = _Color * 10; 
			o.Alpha = sstc.a - pow(abs(.5 - IN.uv_ScreenTex.x) * 2, _FadeX) - pow(abs(.5 - IN.uv_ScreenTex.y) * 2, _FadeY);
			o.Alpha = max(o.Alpha, 0);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
