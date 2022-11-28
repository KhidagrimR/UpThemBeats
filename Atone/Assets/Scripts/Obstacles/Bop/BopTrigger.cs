using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BopTrigger : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public List<Material> materials;


    public GameObject bopVisuel;

    void OnTriggerEnter(Collider col){
        if(col.name == "Player"){
            isTrigger = true;
            bopVisuel.GetComponent<MeshRenderer>().material = materials[1];
            PlayerController.gameObjectColliding = gameObject;
        }
    }

    void OnTriggerExit(Collider col){
        if(col.name == "Player"){
            PlayerController.gameObjectColliding = null;
            isTrigger = false;
            if(!isDestroy)
                bopVisuel.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    public void BopAction(){
        if(isTrigger){
            bopVisuel.SetActive(false);
            isDestroy = true;
        }
            
        else
            print("bop raté");
    }
}
