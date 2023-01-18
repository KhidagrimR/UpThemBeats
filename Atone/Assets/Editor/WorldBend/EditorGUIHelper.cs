using System;
using UnityEngine;
using UnityEditor;


namespace AtoneWorldBendEditor
{
    internal static class EditorGUIHelper 
    {
        public class EditorGUIUtilityLabelWidth : IDisposable
        {
            [SerializeField]
            private float PreviousWidth
            {
                get;
                set; 
            }

            public EditorGUIUtilityLabelWidth(float newWidth)
            {
                PreviousWidth = UnityEditor.EditorGUIUtility.labelWidth;
                UnityEditor.EditorGUIUtility.labelWidth = newWidth;
            }

            public void Dispose()
            {
                UnityEditor.EditorGUIUtility.labelWidth = PreviousWidth;
            }
        }

        public class EditorGUIIndentLevel : IDisposable
        {
            [SerializeField]
            private int PreviousIndent
            {
                get;
                set;
            }

            public EditorGUIIndentLevel(int newIndent)
            {
                PreviousIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = EditorGUI.indentLevel + newIndent;
            }

            public void Dispose()
            {
                EditorGUI.indentLevel = PreviousIndent;
            }
        }
    }
}

