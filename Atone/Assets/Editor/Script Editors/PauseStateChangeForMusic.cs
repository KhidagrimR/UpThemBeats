using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FMODUnity;

[InitializeOnLoadAttribute]
public static class PauseStateChangeForMusic
{
    static PauseStateChangeForMusic() {
        EditorApplication.pauseStateChanged += ToggleMusicPauseOnStateChange;
    }

    private static void ToggleMusicPauseOnStateChange(PauseState pauseState) {
        
        Debug.Log(pauseState);
        Debug.Log("EditorApplication.isPaused ? "+EditorApplication.isPaused);
        //SoundCreator.ToggleMusicPause(EditorApplication.isPaused);
        SoundCreator.MusicFMODInstance.setPaused(EditorApplication.isPaused);

        // Doesn't work as of now :(
        
        
    }
}
