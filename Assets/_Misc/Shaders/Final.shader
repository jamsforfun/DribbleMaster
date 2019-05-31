// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader ".Perso/Final" {
   Properties {
      _MainTex ("Main Texture", 2D) = "white" {}
      _Color ("Main Color", Color) = (1,1,1,1)
      _SpecColor ("Specular Color", Color) = (1,1,1,1)
      _SpecShininess ("Specular Shininess", Range(1.0,100.0)) = 10
      _FresnelPower ("Fresnel Power", Range(0.0, 3.0)) = 1.4
      _FresnelScale ("Frensel Scale", Range (0.0, 1.0)) = 1.0
      _FresnelColor ("Frensel Color", Color) = (1,1,1,1)
   }
   SubShader {
      Pass {
      Tags { "LightMode" = "ForwardBase" }
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
 
         float4 _LightColor0; 
 
         sampler2D _MainTex;
		 float4 _MainTex_ST;
         float4 _Color;
         float4 _SpecColor;
         float _SpecShininess;
		 float _FresnelPower;
		 float _FresnelScale;
		 float4 _FresnelColor;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float refl : TEXCOORD1;
            float4 posWorld : TEXCOORD2;
         };
 
         vertexOutput vert(vertexInput IN) 
         {
            vertexOutput OUT;
 
            OUT.pos = UnityObjectToClipPos(IN.vertex);
            OUT.posWorld = mul(unity_ObjectToWorld, IN.vertex);
            OUT.normal = mul(float4(IN.normal, 0.0), unity_ObjectToWorld).xyz;
            OUT.texcoord = TRANSFORM_TEX(IN.texcoord,_MainTex);

            float3 posWorld = mul(unity_ObjectToWorld, IN.vertex).xyz;
			float3 normWorld = normalize(mul(unity_ObjectToWorld, float4(IN.normal,0.0)).xyz);

			float3 I = posWorld - _WorldSpaceCameraPos.xyz;
			OUT.refl = _FresnelScale * pow(1.0 + dot(normalize(I), normWorld), _FresnelPower);

            return OUT;
         }
 
         float4 frag(vertexOutput IN) : COLOR
         {
         	fixed4 texColor = tex2D(_MainTex, IN.texcoord);

         	float3 normalDirection = normalize(IN.normal);
         	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
         	float3 viewDirection = normalize(_WorldSpaceCameraPos - IN.posWorld.xyz);
         	float3 diffuse = _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

         	float3 specular;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specular = float3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specular = _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _SpecShininess);
            }

            float3 diffuseSpecular = diffuse + specular;
         	float4 lightsColor = UNITY_LIGHTMODEL_AMBIENT + float4(diffuseSpecular,1);
         	float4 finalColor = lightsColor * texColor;
         	return lerp(finalColor, _FresnelColor, IN.refl);
         }
 
         ENDCG
      }
   }
   Fallback "Diffuse"
}