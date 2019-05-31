// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader".Perso/Texture"
{
	Properties
	{
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
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
		float2 texcoord : TEXCOORD0;			//for positionning texture
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 texcoord : TEXCOORD0;			//same
	};

	v2f vert(appdata IN)
	{
		v2f OUT;
		OUT.pos = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		return (OUT);
	}

	fixed4  _Color;
	sampler2D _MainTex;

	fixed4 frag(v2f IN) : COLOR
	{
		fixed4 texColor = tex2D(_MainTex, IN.texcoord);
		return (texColor);
		//return _Color;
	}

		
		ENDCG
		}
	}
}