using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ClawsSoundManager : Singleton<ClawsSoundManager>
{
    public float maxDurationBetweenClaws = 1.0f;
    float lastTimeCheck = 0.0f;
    int soundsClawIndex = 0;
    public EventReference[] clawsSFXArray; 
    public StudioEventEmitter eventClawEmitter;
    

    public void PlayClawSound()
    {
        Debug.Log("playe claw sounds");
        float currentTime = Time.time;

        if(lastTimeCheck < currentTime + maxDurationBetweenClaws)
        {
            //reset
            soundsClawIndex = 0;
        }

        eventClawEmitter.Event = clawsSFXArray[soundsClawIndex].ToString();
        eventClawEmitter.Play();

        if(soundsClawIndex < clawsSFXArray.Length)
            soundsClawIndex++;

        lastTimeCheck = currentTime;
    }
}
