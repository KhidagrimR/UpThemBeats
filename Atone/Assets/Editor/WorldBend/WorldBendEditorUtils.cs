using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AtoneWorldBendEditor
{
    public static class WorldBendEditorUtils
    {
        public static string shaderPropertyName_BendSettings = "_AtoneBendSettings";

        // Le suffixe "_ON" est très important car EnableKeyword/DisableKeyword en ont besoin pour fonctionner:
        // https://docs.unity3d.com/ScriptReference/MaterialPropertyDrawer.html
        static public string shaderKeywordName_CurvedWorldDisabled = "WORLDBEND_DISABLED_ON";
        static public string shaderKeywordName_BendTransformNormal = "WORLDBEND_NORMAL_TRANSFORMATION_ON";

        public static void ShouldTransformNormals(Vector4 propVector, out bool normalTransform) 
        {
            //normalTransform = propVector.y >= 1 ? true : false;
            normalTransform = propVector[1] >= 1 ? true : false;
        }

        // Used when reading the property name
        static bool StringToNormalTransform(string normalTransfromString)
        {
            bool value = false;
            if (string.IsNullOrEmpty(normalTransfromString) == false && normalTransfromString.Length == 1 && normalTransfromString == "1")
                value = true;

            return value;
        }

        public static void UpdateMaterialKeyWords(Material material, bool normalTransform) 
        {
            // guard statement to prevent errors
            if (material == null || material.shader == null) {
                return;
            }

            
            // Update process: disable current-> remove current -> add new -> enable new
            List<string> keyWords = new List<string>(material.shaderKeywords);

            material.DisableKeyword(shaderKeywordName_BendTransformNormal);
            keyWords.Remove(WorldBendEditorUtils.shaderKeywordName_BendTransformNormal);

            if (normalTransform) {
                keyWords.Add(WorldBendEditorUtils.shaderKeywordName_BendTransformNormal);
            }
            material.shaderKeywords = keyWords.ToArray();

            if(normalTransform) {
                material.EnableKeyword(shaderKeywordName_BendTransformNormal);
            }

            // Integer pourra être remplacé par un Vector4 si d'autres paramètres sont rajoutés plus tard
            if(material.HasProperty(shaderPropertyName_BendSettings)) {
                Vector4 prop = material.GetVector(shaderPropertyName_BendSettings);

                prop.y = normalTransform ? 1 : 0;

                material.SetVector(shaderPropertyName_BendSettings,prop);
            }
        }

        // Format in shader is "AffectNormals|1", read number on second index.
        static public bool StringToBendSettings(string label, out bool hasNormalTransform)
        {            
            hasNormalTransform = false;

            if (string.IsNullOrEmpty(label) == false)
            {
                string[] bendSettings = label.Replace("\"", string.Empty).Trim().Split('|');

                if (bendSettings != null)
                {
                    if (bendSettings.Length == 2)
                    {
                        hasNormalTransform = StringToNormalTransform(bendSettings[1]);

                        return true;
                    }
                }
            }

            return false;
        }
        
    }
}


