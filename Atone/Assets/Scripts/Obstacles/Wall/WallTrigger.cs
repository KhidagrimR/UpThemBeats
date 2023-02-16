using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public FMODUnity.StudioEventEmitter fmodStudioEventEmitter;

    public GameObject visualWall;

    public int pointObstacle;

    public Renderer renderer;
    public Color color;
    public float intensity;

    public AnimationTrigger animationTrigger;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
        {
            isTrigger = true;
            //visualWall.GetComponent<MeshRenderer>().material = materials[1];
            PlayerController.gameObjectsColliding.Add(gameObject);
            renderer.materials[0].color = color * Mathf.LinearToGammaSpace(intensity);
            renderer.materials[4].color = color * Mathf.LinearToGammaSpace(intensity);
            renderer.materials[6].color = color * Mathf.LinearToGammaSpace(intensity);
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
            renderer.materials[0].color = color;
            renderer.materials[4].color = color;
            renderer.materials[6].color = color;
        }
    }

    public void WallAction()
    {
        //print("wallAction");
        if (isTrigger && !isDestroy)
        {
            Destroy(visualWall);
            transform.parent.GetComponent<AnimationTrigger>().PlayAnimation(AnimationEnum.Death);  // AnimationTrigger.AnimationEnum.Death
            if (animationTrigger != null)
            {
                animationTrigger.PlayDeathVFX();
            }
            if (fmodStudioEventEmitter != null)
                fmodStudioEventEmitter.Play();

            isDestroy = true;
            PlayerManager.Instance.IncreaseScore(gameObject.GetComponent<BoxCollider>().bounds.extents.z, gameObject.transform.position.z, pointObstacle);
            SequenceManager.Instance.currentSequence.currentAmountOFObstacleDestroyed++;
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.RedwallDestroy);
        }
        else
            print("mur raté");
    }
}
