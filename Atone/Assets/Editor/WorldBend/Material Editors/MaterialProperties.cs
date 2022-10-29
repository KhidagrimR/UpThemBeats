using System;

using UnityEngine;
using UnityEditor;

namespace AtoneWorldBendEditor
{
    static public class MaterialProperties
    {
        public enum STYLE { Foldout, Standard, None  }

        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
            Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
        }
        private static class Label
        {
            public static string mainGroupName = "Atone WorldBend";

            // public static string bendType = "Bend Type";
            // public static string bendID = "Bend ID";
            public static string bendTransformNormal = "Transform Normal";  // Option to choose whether to affect normals or not in deformation

            // public static string renderingMode = "Rendering Mode";
            // public static string renderFace = "Render Face";
        }

        

        static MaterialProperty _AtoneBendSettings = null;

        // static MaterialProperty _BlendMode = null;
        // static MaterialProperty _Cull = null;

        static Material material;

        static bool foldout = true;

        // Pour l'instant il n'y a qu'une propriété custom, mais cette configuration permet d'en rajouter plus tard
        static public void InitWorldBendMaterialProperties(MaterialProperty[] props)
        {
            _AtoneBendSettings = FindProperty(WorldBendEditorUtils.shaderPropertyName_BendSettings, props, false);
        }

        // URP already handles blend setttings and cull
        static public void DrawWorldBendMaterialProperties(MaterialEditor materialEditor, STYLE style)
        {
            switch (style)
            {
                case STYLE.Foldout:
                    {
                        foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, Label.mainGroupName);

                        if (foldout)
                        {
                            if (_AtoneBendSettings != null)
                                materialEditor.ShaderProperty(_AtoneBendSettings, "Bend Settings");   // Label.bendType

                            GUILayout.Space(5);
                        }

                        EditorGUILayout.EndFoldoutHeaderGroup();
                    }
                    break;

                case STYLE.Standard:
                    {
                        EditorGUILayout.LabelField(Label.mainGroupName, EditorStyles.boldLabel);

                        if (_AtoneBendSettings != null)
                            materialEditor.ShaderProperty(_AtoneBendSettings, Label.bendTransformNormal);

                        GUILayout.Space(5);

                    }
                    break;

                case STYLE.None:
                default:
                    {
                        if (_AtoneBendSettings != null)
                            materialEditor.ShaderProperty(_AtoneBendSettings, Label.bendTransformNormal);
                    }
                    break;
            }
        }

        static public void SetKeyWords(Material material)
        {
            if (material.HasProperty(WorldBendEditorUtils.shaderPropertyName_BendSettings))
            {                
                bool normalTransform;

                WorldBendEditorUtils.ShouldTransformNormals(material.GetVector(WorldBendEditorUtils.shaderPropertyName_BendSettings), out normalTransform);

                WorldBendEditorUtils.UpdateMaterialKeyWords(material, normalTransform);
            }
        }

        static public MaterialProperty FindProperty(string propertyName, MaterialProperty[] properties, bool mandatory = true)
        {
            for (int index = 0; index < properties.Length; ++index)
            {
                if (properties[index] != null && properties[index].name == propertyName)
                    return properties[index];
            }

            if (mandatory)
                throw new System.ArgumentException("Could not find MaterialProperty: '" + propertyName + "', Num properties: " + (object)properties.Length);
            else
                return null;
        }
        
    }
}
