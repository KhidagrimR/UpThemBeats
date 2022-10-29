using System;

using UnityEngine;
using UnityEditor;


namespace AtoneWorldBendEditor
{   
    [CanEditMultipleObjects]
    public class WorldBendSeparatorDrawer : MaterialPropertyDrawer
    {
        public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
        {

        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            float fValue;
            if (float.TryParse(label, out fValue))
                return fValue;
            else
                return 0;

        }
    }
}


