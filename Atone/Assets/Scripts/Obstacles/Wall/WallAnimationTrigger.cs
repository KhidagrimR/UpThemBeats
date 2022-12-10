using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimationTrigger : MonoBehaviour
{
    public AnimationTrigger.AnimationEnum animationToTrigger;
    public AnimationTrigger animationTrigger;

    public bool targetPlayerAnim = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Trigger");
        //si c est le joueur
        if(other.CompareTag(PlayerManager.PLAYER_TAG))
        {

            if(targetPlayerAnim) // si l'animation est pour le joueur
            {
                other.GetComponent<PlayerController>().animationTrigger.PlayAnimation(animationToTrigger);
            }
            else // si on cherche ̄ animer l'obstacle
            {
                animationTrigger.PlayAnimation(animationToTrigger);
            }

            
        }
    }
}
