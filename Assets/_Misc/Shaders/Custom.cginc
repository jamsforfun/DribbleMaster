// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

//Sphere-map
float _SpherifyFactor;
float _SphereRadius;
float _SphereOriginDistanceFromCamera;
//Curve-to-left
float _HorizontalCurvature;

inline float4 WorldCurvature (float4 vertex)
{
	float3 worldVertex = mul(unity_ObjectToWorld, vertex).xyz;
	float heightFromGround = worldVertex.y;

	//method 1
	//Drop y-component of vertex pos to get groundPos
	//Get distance of groundPos to sphere origin and normalize it
	//Add the y-component of the vertex pos on top of the sphere radius
	float3 groundPoint = float3(worldVertex.x, 0, worldVertex.z);
	float3 sphereOrigin = float3(0,-1 * _SphereRadius, 0) + float3(_WorldSpaceCameraPos.x, 0, _WorldSpaceCameraPos.z) + float3(0,0,_SphereOriginDistanceFromCamera);
	float3 lengthFromOrigin = groundPoint - sphereOrigin;
	float3 modifiedPosition = sphereOrigin + ((_SphereRadius + heightFromGround) * normalize(lengthFromOrigin));
	
	worldVertex = lerp(worldVertex, modifiedPosition, _SpherifyFactor);
	
	float4 modelVertex = mul(unity_WorldToObject, float4(worldVertex, 1));
	float4 pos = UnityObjectToClipPos(modelVertex);
	
	//Curve-to-left
	//Exponential
	//http://www.wolframalpha.com/input/?i=plot+x*x
//	pos.x += pos.w * pos.w * _HorizontalCurvature;

	//Exponential with offset
	//http://www.wolframalpha.com/input/?i=log+plot+e%5Ex-x*0.1+from+-3+to+-1
//	pos.x += (exp((pos.w-20) * 0.05) - (pos.w - 20) * _HorizontalCurvature) * -0.1;

	//Parabolic with offset
	//http://www.wolframalpha.com/input/?i=parabola
//	float posOffset = pos.w - 20;
//	pos.x += ((posOffset * posOffset) / (_HorizontalCurvature)) * 0.00001;

//	float4 pos = mul(UNITY_MATRIX_MVP, vertex);
	return pos;
}