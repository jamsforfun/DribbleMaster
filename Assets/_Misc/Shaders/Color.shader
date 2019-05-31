// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader".Perso/Color"
{
	Properties
	{
		_Color("Main Color", Color) = (1, 1, 1, 1)
	}

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
		return (OUT);
	}

	fixed4  _Color;

	fixed4 frag(v2f IN) : COLOR
	{
		return _Color;
	}

		
		ENDCG
		}
	}
}