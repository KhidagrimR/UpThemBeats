using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndRoomCameraController : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform playerTransform;
    public GameObject playerMesh;

    public float xRotation = 0f;
    public CinemachineVirtualCamera cmvCamPlayer;
    public CinemachineVirtualCamera cmvCamCinematic;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SetCinematicView();
    }

    public void SetCinematicView()
    {
        cmvCamPlayer.gameObject.SetActive(false);
        cmvCamCinematic.gameObject.SetActive(true);
        playerMesh.SetActive(true);
    }

    public void SetToPlayerView()
    {
        cmvCamPlayer.gameObject.SetActive(true);
        cmvCamCinematic.gameObject.SetActive(false);
        playerMesh.SetActive(false);
    }
    
/*    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update(){
        print("coordinates : " + gameObject.transform.position);
        if (EndRoomPlayerController.canMove){
            float mouseX = Input.GetAxis("MoveCameraX") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("MoveCameraY") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerTransform.Rotate(Vector3.up * mouseX);  
        }
        
    }*/
}
