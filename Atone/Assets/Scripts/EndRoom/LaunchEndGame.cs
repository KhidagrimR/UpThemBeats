using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaunchEndGame : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            other.GetComponent<EndRoomPlayerController>().animator.SetTrigger("FinalDoor");
            other.GetComponent<EndRoomPlayerController>().cameraAnimator.SetTrigger("FinalDoor");
            other.GetComponent<EndRoomPlayerController>().playerCam.GetComponent<EndRoomCameraController>().SetCinematicView();
        }
    }


}
