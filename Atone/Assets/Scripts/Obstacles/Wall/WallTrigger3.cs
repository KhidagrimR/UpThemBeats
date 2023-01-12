using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger3 : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    


    public GameObject visualWall;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
        {
            print("collide");
            isTrigger = true;
            //visualWall.GetComponent<MeshRenderer>().material = materials[1];
            PlayerController.gameObjectsColliding.Add(gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
        {
            PlayerController.gameObjectsColliding.Remove(gameObject);
            isTrigger = false;
            //if (!isDestroy)
            //visualWall.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    public void WallAction()
    {
        print("wallAction");
        if (isTrigger && !isDestroy)
        {
            print("nbHit " + WallManager.nbHit);
            if (WallManager.nbHit < 2){
                print("yolo");
                WallManager.nbHit += 1;
            }
                
            else{
                Destroy(visualWall);
                transform.parent.GetComponent<AnimationTrigger>().PlayAnimation(AnimationTrigger.AnimationEnum.Death);
                isDestroy = true;
            }
            
        }
    }
}
