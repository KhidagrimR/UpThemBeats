#ifndef WORLDBEND_SPIRAL_Z_AXIS_00_INCLUDED
#define WORLDBEND_SPIRAL_Z_AXIS_00_INCLUDED

uniform float3 Z_Deformation_PivotPoint;
uniform float3 Z_Deformation_RotationAxis;
uniform float3 Z_Deformation_BendSize;    
uniform float3 Z_Deformation_BendOffset;
  
                 
#include "../Core/Core.hlsl"        
// Cause de l'erreur de redéfinition avec son appel à Utility.cginc qui appelle une librairie et cause une redéfinition
             
      
////////////////////////////////////////////////////////////////////////////////
// Fonctions de déformation
////////////////////////////////////////////////////////////////////////////////
void WorldBend_Spiral_Z_Axis_00(inout float4 vertexOS) // anciennement WorldBend_Spiral_Z. Renommée pour ne pas se mélanger avec celle de Core.hlsl
{
    WorldBend_Spiral_Z(vertexOS, 
	                    Z_Deformation_PivotPoint,
                        Z_Deformation_RotationAxis,
						Z_Deformation_BendSize,
						Z_Deformation_BendOffset);
}

void WorldBend_Spiral_Z_Axis_00(inout float4 vertexOS, inout float3 normalOS, float4 tangent)
{
    WorldBend_Spiral_Z(vertexOS, 
                        normalOS, 
                        tangent,
                        Z_Deformation_PivotPoint,
                        Z_Deformation_RotationAxis,
						Z_Deformation_BendSize,
						Z_Deformation_BendOffset);
}    

// Conversion en float4 en cas de float3 renseigné
void WorldBend_Spiral_Z_Axis_00(inout float3 vertexOS)
{
    float4 vertex = float4(vertexOS, 1);
    WorldBend_Spiral_Z_Axis_00(vertex);

    vertexOS.xyz = vertex.xyz;
}

void WorldBend_Spiral_Z_Axis_00(inout float3 vertexOS, inout float3 normalOS, float4 tangent)
{
    float4 vertex = float4(vertexOS, 1);
    WorldBend_Spiral_Z_Axis_00(vertex, normalOS, tangent);

    vertexOS.xyz = vertex.xyz;
}  
                  
////////////////////////////////////////////////////////////////////////////////
// Pour shader graph, si on crée un bloc
// En gros, il prend le nom du fichier "WorldBend_Spiral_Z_Axis_00" auquel il rajoute le suffixe "_half" ou "_float" selon la précision
////////////////////////////////////////////////////////////////////////////////

// Version ne recalculant pas les normales
void WorldBend_Spiral_Z_Axis_00_float(float3 vertexOS, out float3 retVertex)
{
    WorldBend_Spiral_Z_Axis_00(vertexOS); 	

    retVertex = vertexOS.xyz;
}

// WorldBend_Spiral_Z_Axis_00_half est généré automatiquement par shadergraph lors de la création du sous-graphe

void WorldBend_Spiral_Z_Axis_00_half(half3 vertexOS, out half3 retVertex) // WorldBend_Spiral_Z_half
{
    WorldBend_Spiral_Z_Axis_00(vertexOS); 	

    retVertex = vertexOS.xyz;
}

// Version recalculant les normales
void WorldBend_Spiral_Z_Axis_00_float(float3 vertexOS, float3 normalOS, float4 tangent, out float3 retVertex, out float3 retNormal)
{
	WorldBend_Spiral_Z_Axis_00(vertexOS, normalOS, tangent); 	

    retVertex = vertexOS.xyz;
    retNormal = normalOS.xyz;
}

void WorldBend_Spiral_Z_Axis_00_half(half3 vertexOS, half3 normalOS, half4 tangent, out half3 retVertex, out float3 retNormal) // WorldBend_Spiral_Z_half
{
	WorldBend_Spiral_Z_Axis_00(vertexOS, normalOS, tangent); 	

    retVertex = vertexOS.xyz;
    retNormal = normalOS.xyz;	
}     

#endif