Shader "Unlit/WallUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FadeX ("Fade X", float) = 4
		_FadeY ("Fade Y", float) = 16
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
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
				float2 pos : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _FadeX;
			float _FadeY;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.pos = float2(o.uv.x, o.uv.y);

				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				o.uv = float2(v.vertex.x, v.vertex.y);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a -= pow(abs(.5 - i.pos.x) * 2, _FadeX);
				col.a -= pow(abs(.5 - i.pos.y) * 2, _FadeY);
				col.a = max(col.a, 0);
				return col;
			}
			ENDCG
		}
	}
}
