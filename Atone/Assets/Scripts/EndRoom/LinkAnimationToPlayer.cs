using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LinkAnimationToPlayer : MonoBehaviour
{
    public EndRoomCameraController cameraController;
    public GameObject player;
    public GameObject positionEndWakeUp;

    public StudioEventEmitter redwall1;

    public StudioEventEmitter redwall2;

    public StudioEventEmitter redwall3;
    public void setIsFinalDoorAnimation()
    {
        EndRoomPlayerController.inAnimationFinalDoor = true;
        EndRoomPlayerController.canMove = false;
    }

    public void setWakeUpEndAnimation()
    {
        EndRoomPlayerController.canMove = true;
        //player.transform.position = positionEndWakeUp.transform.position;
        //player.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        cameraController.SetToPlayerView();
    }

    public void RoomWall1()
    {
        redwall1.Play();
    }
    public void RoomWall2()
    {
        redwall2.Play();
    }
    public void RoomWall3()
    {
        redwall3.Play();
    }
}
