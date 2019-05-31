// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader ".Perso/FullScreenWarp" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "" {}
	_Progress("Progress", Float) = 0
	}

		CGINCLUDE

#include "UnityCG.cginc"

		struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	float _Progress;

	v2f vert(appdata_img v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target{
		fixed2 lookup = i.uv;
	lookup.y += _Progress * 0.1f * sin(5.0f*lookup.x + _Time*100.0f);
	lookup.x += _Progress * 0.1f * cos(5.0f*lookup.y + _Time*100.0f);
	fixed4 orgCol = tex2D(_MainTex, lookup);
	fixed avg = (orgCol.r + orgCol.g + orgCol.b) / 3f;
	fixed3 targ = fixed3(avg, avg, avg);

	return fixed4
	(lerp(orgCol.r, targ.r, 6.0f*_Progress),
		lerp(orgCol.g, targ.g, 6.0f*_Progress),
		lerp(orgCol.b, targ.b, 6.0f*_Progress), 1.0f);
	}

		ENDCG

		Subshader {
		Pass{
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
			ENDCG
		}
	}

	Fallback off
}