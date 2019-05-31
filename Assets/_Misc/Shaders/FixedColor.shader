// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader ".Perso/Fixed Color" {
SubShader
{
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		struct appdata
		{
			float4 vertex : POSITION;
		};
		struct v2f
		{
			float4 pos : SV_POSITION;
		};

		v2f vert(appdata IN)
		{
			v2f OUT;
			OUT.pos = UnityObjectToClipPos(IN.vertex);
			return OUT;
		}

		float4 frag(v2f IN) : SV_TARGET
		{
			return float4(1,0,0,1);
		}
		ENDCG
	}
}
}
