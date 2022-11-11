using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BopTrigger : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isBlue = false;

    public List<Material> materials;


    public GameObject bopVisuel;

    public void Start(){
        InputManager.Instance.onJump += bopAction;
    }
    void OnTriggerEnter(Collider col){
        if(col.name == "Player"){
            isTrigger = true;
            bopVisuel.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    void OnTriggerExit(Collider col){
        if(col.name == "Player"){
            isTrigger = false;
            if(!isBlue)
                bopVisuel.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    void bopAction(){
        if(isTrigger){
            //bopVisuel.GetComponent<MeshRenderer>().material = materials[2];
            bopVisuel.SetActive(false);
            isBlue = true;
        }
            
        else
            print("raté");
    }
}
