using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BopTriggerArrival : MonoBehaviour
{
    public GameObject bopVisuel;
    public GameObject arrivalPointBop;
    public GameObject bopTrigger;


    public float travelTimeToCenter;
    public float timeStayCenter;


    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LaunchArrival());
    }

    public void InitDistance()
    {
        float distance = GameObject.Find("Player").GetComponent<PlayerController>().playerSpeed * travelTimeToCenter;
        gameObject.transform.position = new Vector3(bopTrigger.transform.position.x, bopTrigger.transform.position.y, bopTrigger.transform.position.z - distance);
    }

    public IEnumerator LaunchArrival(){
        bopVisuel.SetActive(true);
        bopVisuel.gameObject.transform.DOMove(arrivalPointBop.gameObject.transform.position, travelTimeToCenter);
        yield return new WaitForSeconds(travelTimeToCenter + timeStayCenter);
        bopVisuel.gameObject.transform.DOMoveY(-20, travelTimeToCenter);
    }

   
}
