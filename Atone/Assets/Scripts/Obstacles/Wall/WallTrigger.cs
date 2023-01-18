using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public FMODUnity.StudioEventEmitter fmodStudioEventEmitter;

    public GameObject visualWall;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG)){
            isTrigger = true;
            //visualWall.GetComponent<MeshRenderer>().material = materials[1];
            PlayerController.gameObjectsColliding.Add(gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG)){
            PlayerController.gameObjectsColliding.Remove(gameObject);
            isTrigger = false;
            //if (!isDestroy)
                //visualWall.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    public void WallAction()
    {
        print("wallAction");
        if (isTrigger && !isDestroy){
            Destroy(visualWall);
            transform.parent.GetComponent<AnimationTrigger>().PlayAnimation(AnimationEnum.Death);  // AnimationTrigger.AnimationEnum.Death

            if(fmodStudioEventEmitter != null) 
                fmodStudioEventEmitter.Play();
                
            isDestroy = true;
        }
        else
            print("mur rat�");
    }
}
