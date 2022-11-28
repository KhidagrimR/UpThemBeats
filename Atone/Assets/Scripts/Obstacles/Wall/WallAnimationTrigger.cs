using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimationTrigger : MonoBehaviour
{
    public AnimationTrigger.AnimationEnum animationToTrigger;
    public AnimationTrigger animationTrigger;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Trigger");
        //si c est le joueur
        if(other.CompareTag(PlayerManager.PLAYER_TAG))
        {
            Debug.Log("tag is ok");
            animationTrigger.PlayAnimation(animationToTrigger);
        }
    }
}
