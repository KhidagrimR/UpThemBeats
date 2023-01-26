using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LaunchAudio : MonoBehaviour {
    public StudioEventEmitter soundEvent;

    /*public void Start() {
        soundEvent.Play();
        soundEvent.Stop();
    }*/

    public void PlaySoundEvent() {
        soundEvent.Play();
    }
}
