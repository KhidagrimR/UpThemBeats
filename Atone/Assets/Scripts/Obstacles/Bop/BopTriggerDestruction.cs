using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BopTriggerDestruction : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public Material materialTrigger;
    public Material initMaterial;

    public ParticleSystem bopExplosion;

    public int pointObstacle;

    public GameObject bopVisuel;
    public StudioEventEmitter SFXArrival;
    public StudioEventEmitter SFXDestroy;

    void OnTriggerEnter(Collider col){
        if(col.name == "Player"){
            isTrigger = true;
            //bopVisuel.GetComponent<MeshRenderer>().materials[2] = materialTrigger;
            PlayerController.gameObjectsColliding.Add(gameObject);
        }
    }

    void OnTriggerExit(Collider col){
        //print("exit");
        if(col.name == "Player"){
            isTrigger = false;
            //bopVisuel.GetComponent<MeshRenderer>().materials[2] = initMaterial;
            //print(bopVisuel.GetComponent<MeshRenderer>().materials[2]);
            PlayerController.gameObjectsColliding.Remove(gameObject);
        }
    }

    public void BopAction(){
        if(isTrigger && !isDestroy){
            bopVisuel.SetActive(false);
            SFXArrival.Stop();
            bopExplosion.transform.position = bopVisuel.transform.position;
            bopExplosion.Play();
            SFXDestroy.Play();
            isDestroy = true;

            PlayerManager.Instance.IncreaseScore(gameObject.GetComponent<BoxCollider>().bounds.extents.z, gameObject.transform.position.z, pointObstacle);
        }
            
        else
            print("bop raté");
    }
}
