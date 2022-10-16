using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundCreator))]
public class SoundCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SoundCreator soundCreator = (SoundCreator) target;

        if(GUILayout.Button("Generate Sound Sheet"))
        {
            soundCreator.ResetMusicSheet();
            soundCreator.SetupVars();
            soundCreator.GenerateMusicSheet();
        }

        if(GUILayout.Button("Reset Sound Sheet"))
        {
            soundCreator.ResetMusicSheet();
        }
    }
}
