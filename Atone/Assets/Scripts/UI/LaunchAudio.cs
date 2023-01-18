using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LaunchAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;
    public EventReference eventRef;

    private void Start() {
        instance = RuntimeManager.CreateInstance(eventRef);
    }

    public void Play() {
        instance.start();
    }
}
