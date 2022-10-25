#ifndef TRANSFORM_ALIASES_INCLUDED
#define TRANSFORM_ALIASES_INCLUDED
// Le but de ce fichier est de n'avoir qu'un seul endroit où changer les noms et chemins, idéalement


// Les noms des fonctions doivent bien correspondre
#include "../Deformation/WorldBend_Spiral_Z_Axis_00.hlsl"
// Vrai Cause de l'erreur "Redefinition of _Time at File [...] (40)" Voir le problème dans Utility.cginc
#define WORLDBEND_TRANSFORM_VERTEX_AND_NORMAL(v, n, t) WorldBend_Spiral_Z(v, n, t);
#define WORLDBEND_TRANSFORM_VERTEX(v)                  WorldBend_Spiral_Z(v);

#endif