using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BopTriggerArrival : MonoBehaviour
{
    public GameObject bopVisuel;
    public GameObject arrivalPointBop;
    
    public float travelTimeToCenter;

    public void Start(){
        gameObject.GetComponent<SpeedDrone>().travelTimeToCenter = travelTimeToCenter;
    }

    public void OnTriggerEnter(Collider other)
    {
        bopVisuel.SetActive(true);
        bopVisuel.gameObject.transform.DOMove(arrivalPointBop.gameObject.transform.position, travelTimeToCenter*2);
    }
}
