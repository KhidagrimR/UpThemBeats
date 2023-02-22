using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideClawSfxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag(PlayerManager.PLAYER_TAG))
        {
            ClawsSoundManager.Instance.PlayClawSound();
        }    
    }
}
