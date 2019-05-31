Shader ".Perso/Gaeel's Distortion" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct v2f {
		float4 pos : SV_POSITION;
		float2 uv_MainTex : TEXCOORD0;
	};

	float4 _MainTex_ST;

	v2f vert(appdata_base v) {
		v2f o;
		//o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		//Gaeel's vertex modification starts here...
		float4 pos = mul(UNITY_MATRIX_MV, v.vertex);

		float distanceSquared = pos.x * pos.x + pos.z * pos.z;
		pos.y += 5 * sin(distanceSquared*_SinTime.x / 1000);
		float y = pos.y;
		float x = pos.x;
		float om = sin(distanceSquared*_SinTime.x / 5000) * _SinTime.x;
		pos.y = x*sin(om) + y*cos(om);
		pos.x = x*cos(om) - y*sin(om);

		o.pos = mul(UNITY_MATRIX_P, pos);
		//...and ends here.

		o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;
	}

	sampler2D _MainTex;

	float4 frag(v2f IN) : COLOR{
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		return c;
	}
		ENDCG
	}
	}
}