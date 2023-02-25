using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class UIMenuAudioHandler : MonoBehaviour
{
    public StudioEventEmitter creditMusic;
    public StudioEventEmitter menuMusic;

    public void PlayCreditMusic()
    {
        StopAllMusics();
        creditMusic.Play();
        Invoke("PlayMenuMusic", 31);
    }

    public void PlayMenuMusic()
    {
        StopAllMusics();
        menuMusic.Play();
    }

    public void StopAllMusics()
    {
        menuMusic.Stop();
        creditMusic.Stop();
    }
}
