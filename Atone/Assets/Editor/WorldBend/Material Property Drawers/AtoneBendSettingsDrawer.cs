using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;


namespace AtoneWorldBendEditor
{
    [CanEditMultipleObjects]
    public class AtoneBendSettingsDrawer : MaterialPropertyDrawer
    {
        static bool useNormalTransform;

        bool updateNormalTransform;
        Material updateMaterial;
        MaterialEditor updateMaterialEditor;


        // void StringToBendSettings(string label, out bool useNormalTransform)
        // {
            // Moved to WorldBendEditorUtils
        // }
        public void Initialize(string label){
            WorldBendEditorUtils.StringToBendSettings(label, out useNormalTransform);
        }

        public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            Material material = (Material)editor.target;
            Initialize(prop.displayName);

            bool normalTransform;
            WorldBendEditorUtils.ShouldTransformNormals(prop.vectorValue, out normalTransform);

            // Vérifier concordance entre shader et ce que l'éditeur affiche.
            if (normalTransform != material.IsKeywordEnabled(WorldBendEditorUtils.shaderKeywordName_BendTransformNormal))
            {
                if (material.IsKeywordEnabled(WorldBendEditorUtils.shaderKeywordName_BendTransformNormal))
                    normalTransform = true;
                else
                    normalTransform = false;
                
                // prop.intValue = useNormalTransform ? (normalTransform ? 1 : 0) : 0;     
                prop.vectorValue = new Vector4( 0, 
                                                (useNormalTransform ? (normalTransform ? 1 : 0) : 0), 
                                                0, 
                                                0);           
            }

            //--------------------------------------------------------------------------------------------------
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;

            position.height = 18;
            using (new EditorGUIHelper.EditorGUIUtilityLabelWidth(0))
            {
                position.height = 18;                
                if (useNormalTransform)
                {
                    position.yMin += 20;
                    position.height = 18;
                    normalTransform = EditorGUI.Toggle(position, "Normal Transform", normalTransform);
                }
            }

            EditorGUI.showMixedValue = false;
            if(EditorGUI.EndChangeCheck())
            {
                prop.intValue = useNormalTransform ? (normalTransform ? 1 : 0) : 0;

                // Trouver un moyen plus solide qu'un lien statique.
                if(File.Exists("Assets/Shaders/WorldBendShaders/Deformation/WorldBend_Spiral_Z_Axis_00.hlsl"))
                {
                    Undo.RecordObjects(editor.targets, "Change Keywords");

                    foreach (Material mat in editor.targets)
                    {
                        WorldBendEditorUtils.UpdateMaterialKeyWords(mat, normalTransform);
                    }
                }
                else
                {
                    //If file does not exist still adjust keyword for normal transformation
                    // position.yMin += 20;
                    // position.height = 36;
                    // EditorGUI.HelpBox(position, "Can't find File!", MessageType.Error);
                    foreach (Material mat in editor.targets)
                    {
                        if (normalTransform)
                            mat.EnableKeyword(WorldBendEditorUtils.shaderKeywordName_BendTransformNormal);
                        else
                            mat.DisableKeyword(WorldBendEditorUtils.shaderKeywordName_BendTransformNormal);
                    }
                }
            }  
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            Material material = (Material)editor.target;
            Initialize(prop.displayName);

            //Read settings
            bool enabledNormalTransform;

            WorldBendEditorUtils.ShouldTransformNormals(prop.vectorValue, out enabledNormalTransform);


            float height = base.GetPropertyHeight(prop, label, editor) * (2 + (useNormalTransform ? 1 : 0)) + 2;
            
            return height;
        }
    }
}

