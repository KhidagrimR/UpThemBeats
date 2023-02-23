using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAnimationToPlayer : MonoBehaviour
{
    public bool isAnimation;

    public Transform cameraAnimation;
    public Transform cameraPlayer;

    public Transform playerAnimation;
    public Transform player;
    void Update()
    {
        if (isAnimation){
            print("coordinates : " + cameraAnimation);
            cameraPlayer = cameraAnimation;
            player = playerAnimation;
        }
    }

    public void setBoolToTrue() {
        isAnimation = true;
    }

    public void setEndAnimation() {
        EndRoomPlayerController.endAnimation = true;
    }
}
