#ifndef TRANSFORM_ALIASES_CGINC
#define TRANSFORM_ALIASES_CGINC
// Le but de ce fichier est de n'avoir qu'un seul endroit où changer les noms et chemins, idéalement


// Les noms des fonctions doivent bien correspondre
#include "../CGINC/WorldBend_Spiral_Z_Axis_00.cginc"
#define WORLDBEND_TRANSFORM_VERTEX_AND_NORMAL(v, n, t) WorldBend_Spiral_Z(v, n, t);
#define WORLDBEND_TRANSFORM_VERTEX(v)                  WorldBend_Spiral_Z(v);

#endif