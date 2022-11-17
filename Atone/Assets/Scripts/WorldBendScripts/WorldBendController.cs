using UnityEngine;

namespace AtoneWorldBend
{
    // Note: si à l'avenir il y a plusieurs catégories de transformations, créer un enum adapté ici (ex: BENDER_TYPE)

    // Actuellement, le shader transforme uniquement les objets en fonction de leur position sur l'axe Z, 
    // et uniquement s'ils se trouvent du côté positif du point pivot (càd que pos.z - pivot.z >= 0)

    // Z négatif dans le dos par défaut est une convention :     
    // https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/building-basic-perspective-projection-matrix

    [ExecuteAlways]
    public class WorldBendController : MonoBehaviour
    {
        #region VARIABLES
        // Certaines passeront en private serialisées plus tard quand les accesseurs définitifs seront réalisés


        public enum AXIS_TYPE { Transform, Custom, CustomNormalized }
        // Transform : le même que le point pivot, où (0,0,-1) si pas de point pivot
        // Custom : à définir manuellement
        // CustomNormalised :  le vecteur de direction conserve une norme de 1. Principe similaire à celui d'un widget de rotation d'éditeur 3D

        public Transform bendPivotPoint; public Vector3 bendPivotPointPosition;
        public AXIS_TYPE bendRotationAxisType; 
        [SerializeField]private Vector3 bendRotationAxis;

        [SerializeField]private float bendVerticalSize, bendVerticalOffset;
        [SerializeField]private float bendHorizontalSize, bendHorizontalOffset;
        [SerializeField]private float bendCurvatureSize, bendCurvatureOffset;

        public bool disableInEditor = false;

        int materialPropertyID_PivotPoint;
        int materialPropertyID_RotationAxis;
        int materialPropertyID_BendSize;
        int materialPropertyID_BendOffset;        

        #endregion

        #region PROPERTIES
        
        public float BendVerticalSize {get => bendVerticalSize; set => bendVerticalSize = value;}
        public float BendVerticalOffset {get => bendVerticalOffset; set => bendVerticalOffset = value;}
        public float BendHorizontalSize {get => bendHorizontalSize; set => bendHorizontalSize = value;}
        public float BendHorizontalOffset {get => bendHorizontalOffset; set => bendHorizontalOffset = value;}
        public Vector3 BendCurvatureAxis { get => bendRotationAxis; set => bendRotationAxis = value; }
        public float BendCurvatureSize {get => bendCurvatureSize; set => bendCurvatureSize = value;}
        public float BendCurvatureOffset {get => bendCurvatureOffset; set => bendCurvatureOffset = value;}

        #endregion

        #region MONOBEHAVIOUR_CALLBACKS
        void OnEnable()
        {
            EnableBend();
        }

        void Start()
        {
            GenerateShaderPropertyIDs();
        }

        void Update()
        {
            // Si l'update est gérée ailleurs plus tard, il faudra modifier cette partie

            UpdateShaderdata();
        }

        void OnDisable()
        {
            DisableBend();
        }

        void OnDestroy()
        {
            DisableBend();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(bendPivotPointPosition, bendRotationAxis.normalized * 10);            
        }
        #endregion

        #region METHODS
        void UpdateShaderdata()
        {
            if (isActiveAndEnabled == true)
            {                
                if (disableInEditor && Application.isEditor && Application.isPlaying == false)
                {
                    DisableShaderUpdate();
                }
                else
                {
                    if (bendPivotPoint != null)
                        { bendPivotPointPosition = bendPivotPoint.transform.position; }
                    
                    switch (bendRotationAxisType)
                    {
                        case AXIS_TYPE.Transform:
                            {
                                if (bendPivotPoint == null)
                                {
                                    // Rappel: On assume ici que la profondeur ici est l'axe Z, et que le perso se déplace vers Z positif                                    
                                    bendRotationAxis = Vector3.back; 
                                    break;
                                }
                                else
                                {
                                    bendRotationAxis = bendPivotPoint.forward;
                                }
                            }
                            break;
                        case AXIS_TYPE.Custom:
                            break;

                        case AXIS_TYPE.CustomNormalized:
                            bendRotationAxis = bendRotationAxis.normalized;
                            break;
                    }
                    // un vecteur envoyé par SetGlobalVector est reçu et appliqué par tous les shaders possédant cette variable, même si celle-ci n'est pas exposée dans leurs propriétés
                    Shader.SetGlobalVector(materialPropertyID_PivotPoint, bendPivotPointPosition);
                    Shader.SetGlobalVector(materialPropertyID_RotationAxis, bendRotationAxis);
                    Shader.SetGlobalVector(materialPropertyID_BendSize, new Vector3(bendCurvatureSize, bendVerticalSize, bendHorizontalSize));
                    Shader.SetGlobalVector(materialPropertyID_BendOffset, new Vector3(bendCurvatureOffset, bendVerticalOffset, bendHorizontalOffset));
                }
            }
        }

        void DisableShaderUpdate()
        {
            Shader.SetGlobalVector(materialPropertyID_PivotPoint, Vector3.zero);
            Shader.SetGlobalVector(materialPropertyID_RotationAxis, Vector3.zero);

            Shader.SetGlobalVector(materialPropertyID_BendSize, Vector3.zero);
            //Shader.SetGlobalFloat(materialPropertyID_BendSize, 0);

            Shader.SetGlobalVector(materialPropertyID_BendOffset, Vector3.zero);
            //Shader.SetGlobalFloat(materialPropertyID_BendOffset, 0);
        }

        public void EnableBend()
        {
            GenerateShaderPropertyIDs();

            UpdateShaderdata();
        }

        public void DisableBend()
        {
            GenerateShaderPropertyIDs();

            DisableShaderUpdate();
        }

        void GenerateShaderPropertyIDs()
        {
            materialPropertyID_PivotPoint = Shader.PropertyToID("Z_Deformation_PivotPoint");
            materialPropertyID_RotationAxis = Shader.PropertyToID("Z_Deformation_RotationAxis");
            materialPropertyID_BendSize = Shader.PropertyToID("Z_Deformation_BendSize");
            materialPropertyID_BendOffset = Shader.PropertyToID("Z_Deformation_BendOffset");
        }
        #endregion

        #region SETTER_METHODS
        // Des setters style Java. Remplacées par des propriétés C#

        // public void SetBendVerticalSize(float value)
        // {
        //     bendVerticalSize = value;
        // }

        // public void SetBendVerticalOffset(float value)
        // {
        //     bendVerticalOffset = value;
        // }

        // public void SetBendHorizontalSize(float value)
        // {
        //     bendHorizontalSize = value;
        // }

        // public void SetBendHorizontalOffset(float value)
        // {
        //     bendHorizontalOffset = value;
        // }

        // public void SetBendCurvatureSize(float value)
        // {
        //     bendCurvatureSize = value;
        // }

        // public void SetBendCurvatureOffset(float value)
        // {
        //     bendCurvatureOffset = value;
        // }

        // public void SetBendRotationAxis(Vector3 value)
        // {
        //     bendRotationAxis = value;
        // }

        #endregion
    }
}