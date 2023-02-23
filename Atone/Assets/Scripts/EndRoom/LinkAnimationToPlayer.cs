using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAnimationToPlayer : MonoBehaviour
{
    public void setIsFinalDoorAnimation() {
        EndRoomPlayerController.inAnimationFinalDoor = true;
        EndRoomPlayerController.canMove = false;
    }

    public void setWakeUpEndAnimation() {
        EndRoomPlayerController.canMove = true;
    }
}
