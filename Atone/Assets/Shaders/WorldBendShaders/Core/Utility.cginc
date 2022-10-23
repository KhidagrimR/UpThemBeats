#ifndef ATONE_UTILITY_CGINC
#define ATONE_UTILITY_CGINC

#ifndef SCRIPTABLE_RENDER_PIPELINE
// Vérification que l'on est bien dans un pipeline interne de Unity (BRP/URP/HDRP)
	#ifndef UNITY_CG_INCLUDED
		#include "UnityCG.cginc"	//'UnityCG.cginc' contien des fonctionnalités utilisées par ce fichier.
	#endif
#endif

float3 Custom_ObjectToWorld(float4 vertexOS)
{
	return mul(unity_ObjectToWorld, vertexOS).xyz;
}

float3 Custom_WorldToObject(float4 vertexWS, float HDRPCoef)
{
	return mul(unity_WorldToObject, vertexWS).xyz;
}

float3 Custom_ObjectToWorldNormal(float3 normalOS)
{
	return UnityObjectToWorldNormal(normalOS);
}

float3 Custom_WorldToObjectNormal(float3 normalWS)
{
	return mul((float3x3)unity_WorldToObject, normalWS);
}
 
float3 Custom_ObjectToWorldTangent(float3 tangentOS)
{
	return UnityObjectToWorldDir(tangentOS);
} 

//----------------------------------------------------------------------------------------------------------------

inline float SmoothTwist(float x, float scale)
{
	float d = x / scale;
	float s = d * d;
	float smooth = lerp(s, d, s) * scale;

	return x < scale ? smooth : x;
}

inline float3 SmoothTwist(float3 x, float scale)
{
	float3 d = x / scale;
	float3 s = d * d;
	float3 smooth = lerp(s, d, s) * scale;

	return float3(x.x < scale ? smooth.x : x.x, x.y < scale ? smooth.y : x.y, x.z < scale ? smooth.z : x.z);
}

//----------------------------------------------------------------------------------------------------------------

inline void RotateVertex(inout float3 vertex, float3 pivot, float3 axis, float angle)
{
	//degré vers radians / 2, soit PI/360. multiplier par une constante est plus rapide
	angle *= 0.00872664625;


	float sinA, cosA;
	sincos(angle, sinA, cosA);

	float3 q = axis * sinA;

	//Retirer le déplacement vis à vis du pivot, effectuer la rotation, puis rétablir le décalage initial
	vertex -= pivot;
	vertex += cross(q, cross(q, vertex) + vertex * cosA) * 2;
	vertex += pivot;		
}


#endif