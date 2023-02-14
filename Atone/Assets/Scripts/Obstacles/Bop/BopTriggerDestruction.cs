using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.VFX;

public class BopTriggerDestruction : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public Material materialTrigger;
    public Material initMaterial;

    public VisualEffect bopExplosion;
    public ParticleSystem bopBreakable;

    public int pointObstacle;

    public GameObject bopVisuel;

    public GameObject SFXManager;
    
    void OnTriggerEnter(Collider col){
        if (col.CompareTag(PlayerManager.PLAYER_TAG)){ 
            isTrigger = true;
            //bopVisuel.GetComponent<Renderer>().materials[2] = materialTrigger;
            //print("pink"+materialTrigger);
            //print(bopVisuel.GetComponent<MeshRenderer>().materials[2]);
            bopBreakable.Play();
            PlayerController.gameObjectsColliding.Add(gameObject);
        }
    }

    void OnTriggerExit(Collider col){
        //print("exit");
        if(col.name == "Player"){
            isTrigger = false;
            //bopVisuel.GetComponent<Renderer>().materials[2] = initMaterial;
            //print(bopVisuel.GetComponent<MeshRenderer>().materials[2]);
            bopBreakable.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            PlayerController.gameObjectsColliding.Remove(gameObject);
        }
    }

    public void BopAction(){
        if(isTrigger && !isDestroy){
            bopVisuel.SetActive(false);
            SFXManager.GetComponent<SFXManagerBop>().loadDataSound.Stop();
            bopExplosion.transform.position = bopVisuel.transform.position;
            bopExplosion.Play();
            SFXManager.GetComponent<SFXManagerBop>().destructionSound.Play();
            isDestroy = true;

            PlayerManager.Instance.IncreaseScore(gameObject.GetComponent<BoxCollider>().bounds.extents.z, gameObject.transform.position.z, pointObstacle);
            SequenceManager.Instance.currentSequence.currentAmountOFObstacleDestroyed++;
        }
            
        else
            print("bop raté");
    }
}
