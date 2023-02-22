using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SlideClawSfxTrigger : MonoBehaviour
{
    public StudioEventEmitter eventClawEmitter;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag(PlayerManager.PLAYER_TAG))
        {
            eventClawEmitter.EventReference = ClawsSoundManager.Instance.GetClawSound();
            eventClawEmitter.Play();
        }    
    }
}
