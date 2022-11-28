#ifndef ATONE_CORE_INCLUDED
#define ATONE_CORE_INCLUDED
// Pratique en hlsl pour éviter les doublons. Appelés en anglais "Guard keywords"

#include "Utility.hlsl"


////////////////////////////////////////////////////////////////////////////////
// Ici sont les calculs pour réaliser les distortions (Horizontale, verticale et torsion)
//////////////////////////////////////////////////////////////////////////////// 

void WorldBend_Spiral_Z(inout float4 inVertexOS, float3 pivotPoint, float3 rotationAxis, float3 bendSize, float3 bendOffset)
{
	float3 positionWS = Custom_ObjectToWorld(inVertexOS);
    positionWS -= pivotPoint;


    float d = max(0, positionWS.z - bendOffset.x);
	d = SmoothTwist(d, 100);
    float angle = bendSize.x * d;         
            
    RotateVertex(positionWS, pivotPoint, rotationAxis, angle);


    float2 offset = max(float2(0, 0), positionWS.zz - bendOffset.yz);
    offset *= offset;
    positionWS += float3(bendSize.z * offset.y, bendSize.y * offset.x, 0.0f) * 0.001;



    positionWS += pivotPoint;

    inVertexOS.xyz = Custom_WorldToObject(float4(positionWS, 1), 1);
}

void WorldBend_Spiral_Z(inout float4 inVertexOS, inout float3 inNormalOS, float4 tangentOS, float3 pivotPoint, float3 rotationAxis, float3 bendSize, float3 bendOffset)
{
	float3 positionWS = Custom_ObjectToWorld(inVertexOS);
    float3 normalWS   = Custom_ObjectToWorldNormal(inNormalOS);
    float3 tangentWS  = Custom_ObjectToWorldTangent(tangentOS.xyz);
    float3 binormalWS = cross(tangentWS, normalWS);

    positionWS -= pivotPoint;


    float3 v0 = positionWS;
    float3 v1 = v0 + tangentWS;
    float3 v2 = v0 + binormalWS;


    float3 d = max(0, float3(v0.z, v1.z, v2.z) - bendOffset.xxx);
	d = SmoothTwist(d, 100);
    float3 angle = bendSize.xxx * d;         
            

    RotateVertex(v0, pivotPoint, rotationAxis, angle.x);
    float2 offset = max(float2(0, 0), v0.zz - bendOffset.yz);
    offset *= offset;
    v0 += float3(bendSize.z * offset.y, bendSize.y * offset.x, 0.0f) * 0.001;


    RotateVertex(v1, pivotPoint, rotationAxis, angle.y);
    offset = max(float2(0, 0), v1.zz - bendOffset.yz);
    offset *= offset;
    v1 += float3(bendSize.z * offset.y, bendSize.y * offset.x, 0.0f) * 0.001;
      

    RotateVertex(v2, pivotPoint, rotationAxis, angle.z);
    offset = max(float2(0, 0), v2.zz - bendOffset.yz);
    offset *= offset;
    v2 += float3(bendSize.z * offset.y, bendSize.y * offset.x, 0.0f) * 0.001;



    v0 += pivotPoint;
    v1 += pivotPoint;
    v2 += pivotPoint;

    inVertexOS.xyz = Custom_WorldToObject(float4(v0, 1), 1);

    inNormalOS = Custom_WorldToObjectNormal(normalize(cross(v2 - v0, v1 - v0)));
}

#endif