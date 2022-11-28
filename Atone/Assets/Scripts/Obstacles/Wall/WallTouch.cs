using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTouch : MonoBehaviour
{
    public void OnTriggerEnter(Collider col){
        if(col.CompareTag(PlayerManager.PLAYER_TAG)){
            print("perte d'une vie");
        }
    }
}
