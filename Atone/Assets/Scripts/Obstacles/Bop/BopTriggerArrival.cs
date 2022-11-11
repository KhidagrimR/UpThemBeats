using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BopTriggerArrival : MonoBehaviour
{
    public GameObject bopVisuel;
    public GameObject arrivalPointBop;
    public void OnTriggerEnter(Collider other)
    {
        bopVisuel.SetActive(true);
        bopVisuel.gameObject.transform.DOMove(arrivalPointBop.gameObject.transform.position, 2.5f);
    }
}
