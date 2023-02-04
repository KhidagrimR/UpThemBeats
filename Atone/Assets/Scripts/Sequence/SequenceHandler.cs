using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SequenceHandler : MonoBehaviour
{
    // Faire en sorte de pouvoir assigner une piste audio de FMOD directement dans le prefab de la sequence
    public Transform centerRoad;
    public EventReference musicFMODEvent; //FMOD Event reference.  

    public void Init()
    {

    }

    public bool CheckLoopConditions()
    {
        return true;
    }
}
