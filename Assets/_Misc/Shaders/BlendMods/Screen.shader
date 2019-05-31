// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader".Perso/BlendMods/Screen"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		GrabPass{}

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "blendModes.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(IN.vertex);
				o.screenPos = o.vertex;
				o.texcoord = IN.texcoord;
				o.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
					o.vertex = UnityPixelSnap(o.vertex);
				#endif

				return o;
			}

			sampler2D _MainTex;
			sampler2D _GrabTexture;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

				float2 grabTexcoord = IN.screenPos.xy / IN.screenPos.w;
				grabTexcoord.x = (grabTexcoord.x + 1.0) / 2;
				grabTexcoord.y = (grabTexcoord.y + 1.0) / 2;

				#if UNITY_UV_STARTS_AT_TOP
					grabTexcoord.y = 1.0 - grabTexcoord.y;
				#endif

				fixed4 finalGrabText = tex2D(_GrabTexture, grabTexcoord);

				return Screen(finalGrabText, color);
			}

			ENDCG
		}
	}

	Fallback "Sprites/Default"
}