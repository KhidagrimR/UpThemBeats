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

        //Debug.Log("Current Time = "+currentTime+", lastTimeCheck = "+lastTimeCheck);

        if(lastTimeCheck + maxDurationBetweenClaws < currentTime )
        {
            //reset
            Debug.Log("RESET");
            soundsClawIndex = 0;
        }

        EventReference fmodRef = clawsSFXArray[soundsClawIndex];

        if(soundsClawIndex < clawsSFXArray.Length - 1)
            soundsClawIndex++;

        lastTimeCheck = currentTime;
        return fmodRef;
    }
}
