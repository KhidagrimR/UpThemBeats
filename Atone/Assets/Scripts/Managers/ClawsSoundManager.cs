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
    [InspectorReadOnly]
    public int soundsClawDeployIndex = 0;

    [Header("Claws Cross")]
    public EventReference[] clawsSFXArray; 

    public EventReference GetClawSoundCross()
    {
        float currentTime = Time.time;

        //Debug.Log("Current Time = "+currentTime+", lastTimeCheck = "+lastTimeCheck);

        if(lastTimeCheck + maxDurationBetweenClaws < currentTime )
        {
            //reset
            soundsClawIndex = 0;
        }

        EventReference fmodRef = clawsSFXArray[soundsClawIndex];

        if(soundsClawIndex < clawsSFXArray.Length - 1)
            soundsClawIndex++;

        lastTimeCheck = currentTime;
        return fmodRef;
    }

    float lastTimeCheckDeploy = 0.0f;
    [Header("Claws Deploy")]
    public EventReference[] clawsSFXDeployArray; 

    public EventReference GetClawSoundStart()
    {
        float currentTime = Time.time;

        //Debug.Log("Current Time = "+currentTime+", lastTimeCheck = "+lastTimeCheck);

        if(lastTimeCheckDeploy + maxDurationBetweenClaws < currentTime )
        {
            //reset
            soundsClawDeployIndex = 0;
        }

        EventReference fmodRef = clawsSFXDeployArray[soundsClawDeployIndex];

        if(soundsClawDeployIndex < clawsSFXDeployArray.Length - 1)
            soundsClawDeployIndex++;

        lastTimeCheckDeploy = currentTime;
        return fmodRef;
    }
}
