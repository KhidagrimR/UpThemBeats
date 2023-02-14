using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SequenceHandler : MonoBehaviour
{
    // Faire en sorte de pouvoir assigner une piste audio de FMOD directement dans le prefab de la sequence
    public Transform[] lanes;
    public EventReference musicFMODEvent; //FMOD Event reference.  


    // minimum number of obstacle to destroy on loop sequences to let the player continue
    public float minAmountOfObstacleDestroyed;

    [InspectorReadOnly]
    public float currentAmountOFObstacleDestroyed;

    public void Init()
    {
        currentAmountOFObstacleDestroyed = 0f;
    }

    public bool CheckLoopConditions()
    {
        if(currentAmountOFObstacleDestroyed >= minAmountOfObstacleDestroyed)
            return false; // the sequence will not loop
        
        return true; // the sequence will loop
    }
}
