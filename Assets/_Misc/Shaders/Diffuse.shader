// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader".Perso/Diffuse"
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
		float3 normal: NORMAL;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 texcoord : TEXCOORD0;			//same
		float3 normal : NORMAL;
	};

	v2f vert(appdata IN)
	{
		v2f OUT;
		OUT.pos = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.normal = mul(float4(IN.normal, 0.0), unity_ObjectToWorld).xyz;
		return (OUT);
	}

	fixed4  _Color;
	sampler2D _MainTex;
	float4 _LightColor0;

	fixed4 frag(v2f IN) : COLOR
	{
		fixed4 texColor = tex2D(_MainTex, IN.texcoord);
		float3 normalDirection = normalize(IN.normal);
		float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);//little hardcode
		float3 diffuse = _LightColor0.rgb * max(0.0, dot(normalDirection, lightDirection));

		return (_Color * texColor * float4(diffuse, 1));
	}

		
		ENDCG
		}
	}
}