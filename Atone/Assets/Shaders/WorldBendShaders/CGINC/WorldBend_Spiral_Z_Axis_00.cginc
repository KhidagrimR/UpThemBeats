#ifndef WORLDBEND_SPIRAL_Z_AXIS_00_CGINC
#define WORLDBEND_SPIRAL_Z_AXIS_00_CGINC

uniform float3 Z_Deformation_PivotPoint;
uniform float3 Z_Deformation_RotationAxis;
uniform float3 Z_Deformation_BendSize;    
uniform float3 Z_Deformation_BendOffset;
  
                 
#include "../Core/Core.cginc"                           
             
      
////////////////////////////////////////////////////////////////////////////////
// Fonctions de déformation
////////////////////////////////////////////////////////////////////////////////
void WorldBend_Spiral_Z(inout float4 vertexOS)
{
    WorldBend_Spiral_Z(vertexOS, 
	                    Z_Deformation_PivotPoint,
                        Z_Deformation_RotationAxis,
						Z_Deformation_BendSize,
						Z_Deformation_BendOffset);
}

void WorldBend_Spiral_Z(inout float4 vertexOS, inout float3 normalOS, float4 tangent)
{
    WorldBend_Spiral_Z(vertexOS, 
                        normalOS, 
                        tangent,
                        Z_Deformation_PivotPoint,
                        Z_Deformation_RotationAxis,
						Z_Deformation_BendSize,
						Z_Deformation_BendOffset);
}    

void WorldBend_Spiral_Z(inout float3 vertexOS)
{
    float4 vertex = float4(vertexOS, 1);
    WorldBend_Spiral_Z(vertex);

    vertexOS.xyz = vertex.xyz;
}

void WorldBend_Spiral_Z(inout float3 vertexOS, inout float3 normalOS, float4 tangent)
{
    float4 vertex = float4(vertexOS, 1);
    WorldBend_Spiral_Z(vertex, normalOS, tangent);

    vertexOS.xyz = vertex.xyz;
}  
                  
////////////////////////////////////////////////////////////////////////////////
// Pour shader graph, si on crée un bloc
////////////////////////////////////////////////////////////////////////////////
void WorldBend_Spiral_Z_float(float3 vertexOS, out float3 retVertex)
{
    WorldBend_Spiral_Z(vertexOS); 	

    retVertex = vertexOS.xyz;
}

void WorldBend_Spiral_Z_half(half3 vertexOS, out half3 retVertex)
{
    WorldBend_Spiral_Z(vertexOS); 	

    retVertex = vertexOS.xyz;
}

void WorldBend_Spiral_Z_float(float3 vertexOS, float3 normalOS, float4 tangent, out float3 retVertex, out float3 retNormal)
{
	WorldBend_Spiral_Z(vertexOS, normalOS, tangent); 	

    retVertex = vertexOS.xyz;
    retNormal = normalOS.xyz;
}

void WorldBend_Spiral_Z_half(half3 vertexOS, half3 normalOS, half4 tangent, out half3 retVertex, out float3 retNormal)
{
	WorldBend_Spiral_Z(vertexOS, normalOS, tangent); 	

    retVertex = vertexOS.xyz;
    retNormal = normalOS.xyz;	
}     

#endif