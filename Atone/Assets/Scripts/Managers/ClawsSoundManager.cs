using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ClawsSoundManager : Singleton<ClawsSoundManager>
{
    public float maxDurationBetweenClaws = 1.0f;
    float lastTimeCheck = 0.0f;
    [InspectorReadOnly]
    public int soundsClawIndex = 0;
    public EventReference[] clawsSFXArray; 

    public EventReference GetClawSound()
    {
        float currentTime = Time.time;

        if(lastTimeCheck < currentTime + maxDurationBetweenClaws)
        {
            //reset
            soundsClawIndex = 0;
        }

        EventReference fmodRef = clawsSFXArray[soundsClawIndex];

        if(soundsClawIndex < clawsSFXArray.Length)
            soundsClawIndex++;

        lastTimeCheck = currentTime;
        return fmodRef;
    }
}
