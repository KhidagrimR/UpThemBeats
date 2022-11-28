using UnityEngine;
using UnityEditor;

namespace AtoneWorldBendEditor
{
    [CustomEditor(typeof(AtoneWorldBend.CameraBendCorrection))]
    [CanEditMultipleObjects]

    public class CameraBendCorrectionEditor : Editor
    {
        SerializedProperty matrixType;
        SerializedProperty fieldOfView;
        SerializedProperty size;
        SerializedProperty nearClipPlaneSameAsCamera;
        SerializedProperty nearClipPlaneCustom;
        SerializedProperty visualizeInEditor;
        SerializedProperty alignYRotationWithCamera;
        SerializedProperty customLookDirection;


        void OnEnable()
        {
            matrixType = serializedObject.FindProperty("matrixType");
            fieldOfView = serializedObject.FindProperty("fieldOfView");
            size = serializedObject.FindProperty("size");
            nearClipPlaneSameAsCamera = serializedObject.FindProperty("nearClipPlaneSameAsCamera");
            nearClipPlaneCustom = serializedObject.FindProperty("nearClipPlaneCustom");
            visualizeInEditor = serializedObject.FindProperty("visualizeInEditor");
            alignYRotationWithCamera = serializedObject.FindProperty("alignYRotationWithCamera");
            customLookDirection = serializedObject.FindProperty("customLookDirection");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(5);
            EditorGUILayout.PropertyField(matrixType);

            if (matrixType.enumValueIndex == (int)AtoneWorldBend.CameraBendCorrection.MATRIX_TYPE.Perspective)
                EditorGUILayout.PropertyField(fieldOfView);
            else
                EditorGUILayout.PropertyField(size);

            if (matrixType.enumValueIndex == (int)AtoneWorldBend.CameraBendCorrection.MATRIX_TYPE.Orthographic)
            {
                nearClipPlaneSameAsCamera.boolValue = EditorGUILayout.IntPopup("Near Clip Plane", nearClipPlaneSameAsCamera.boolValue ? 1 : 0, new string[] { "Custom", "Same As Camera" }, new int[] { 0, 1 }) == 1 ? true : false;
                if (nearClipPlaneSameAsCamera.boolValue == false)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        EditorGUILayout.PropertyField(nearClipPlaneCustom, new GUIContent("Custom value"));
                    }
                }
            }

            GUILayout.Space(5);

            EditorGUILayout.PropertyField(alignYRotationWithCamera);
            
            if(alignYRotationWithCamera.boolValue == false)
                using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        EditorGUILayout.PropertyField(customLookDirection, new GUIContent("Direction"));
                    }



            EditorGUILayout.PropertyField(visualizeInEditor);


            serializedObject.ApplyModifiedProperties();
        }
    }
}


