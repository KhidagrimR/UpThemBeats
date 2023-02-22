using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ClawAnimationTrigger : MonoBehaviour
{
    // public AnimationTrigger.AnimationEnum animationToTrigger;
    public AnimationEnum animationToTrigger;
    public AnimationTrigger animationTrigger;

    public bool targetPlayerAnim = false;
    public StudioEventEmitter eventClawEmitter;


    public void TriggerClawAnimation()
    {
        //Debug.Log("trigger Anim");
            
        if(targetPlayerAnim) // si l'animation est pour le joueur
        {
            PlayerManager.Instance.playerController.animationTrigger.PlayAnimation(animationToTrigger);
        }
        else // si on cherche ̄ animer l'obstacle
        {
            animationTrigger.PlayAnimation(animationToTrigger);
            animationTrigger.PlayArrivalVFX();

            eventClawEmitter.EventReference = ClawsSoundManager.Instance.GetClawSoundStart();
            eventClawEmitter.Play();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(PlayerManager.PLAYER_TAG))
        {
            TriggerClawAnimation();
        }
    }
}