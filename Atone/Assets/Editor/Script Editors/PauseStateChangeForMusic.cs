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
        
        // Debug.Log(pauseState);
        // Debug.Log("EditorApplication.isPaused ? "+EditorApplication.isPaused);
        
        RuntimeManager.PauseAllEvents(pauseState == PauseState.Paused);
    }
}
