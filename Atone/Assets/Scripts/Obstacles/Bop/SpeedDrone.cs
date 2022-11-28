using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpeedDrone : MonoBehaviour
{
    [NonSerialized]
    public float travelTimeToCenter;
    public GameObject bopTrigger;



    public void InitDistance(){
        print("travelTimeToCenter " + travelTimeToCenter);
        float distance = GameObject.Find("Player").GetComponent<PlayerController>().playerSpeed * travelTimeToCenter;
        distance /= 2;

        print("distance " + distance);
        print("position before " + gameObject.transform.position);
        gameObject.transform.position = new Vector3(bopTrigger.transform.position.x, bopTrigger.transform.position.y, bopTrigger.transform.position.z - distance);
        print("position after " + gameObject.transform.position);
    }


}
