using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public bool isTrigger = false;
    public bool isDestroy = false;

    public List<Material> materials;


    public GameObject visualWall;

    public void Start()
    {
        InputManager.Instance.onDestroy += wallAction;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player")
        {
            isTrigger = true;
            visualWall.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Player")
        {
            isTrigger = false;
            if (!isDestroy)
                visualWall.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    void wallAction()
    {
        if (isTrigger)
        {
            Destroy(visualWall);
            isDestroy = true;
        }

        else
            print("raté");
    }
}
