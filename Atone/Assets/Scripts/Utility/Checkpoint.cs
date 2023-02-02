using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        if (other.CompareTag(PlayerManager.PLAYER_TAG)){
            PlayerController.checkpoint = gameObject.transform.position;
            StartCoroutine(DisplayScore.Instance.DisplayScoreWhenPlay(PlayerManager.score));
        }
            
    }
}
