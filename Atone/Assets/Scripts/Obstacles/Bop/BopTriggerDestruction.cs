using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BopTriggerDestruction : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public Material materialTrigger;
    public Material initMaterial;



    public GameObject bopVisuel;

    void OnTriggerEnter(Collider col){
        if(col.name == "Player"){
            isTrigger = true;
            bopVisuel.GetComponent<MeshRenderer>().materials[2] = materialTrigger;
            PlayerController.gameObjectsColliding.Add(gameObject);
        }
    }

    void OnTriggerExit(Collider col){
        //print("exit");
        if(col.name == "Player"){
            isTrigger = false;
            bopVisuel.GetComponent<MeshRenderer>().materials[2] = initMaterial;
            //print(bopVisuel.GetComponent<MeshRenderer>().materials[2]);
            PlayerController.gameObjectsColliding.Remove(gameObject);
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
