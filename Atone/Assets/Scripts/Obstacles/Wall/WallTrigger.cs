using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public List<Material> materials;


    public GameObject visualWall;

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player"){
            isTrigger = true;
            visualWall.GetComponent<MeshRenderer>().material = materials[1];
            PlayerController.gameObjectCollinding = gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Player"){
            PlayerController.gameObjectCollinding = null;
            isTrigger = false;
            if (!isDestroy)
                visualWall.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    public void WallAction()
    {
        print("wallAction");
        if (isTrigger){
            Destroy(visualWall);
            isDestroy = true;
        }
        else
            print("mur raté");
    }
}
