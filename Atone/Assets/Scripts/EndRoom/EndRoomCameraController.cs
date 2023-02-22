using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomCameraController : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform playerTransform;

    public float xRotation = 0f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update(){
        float mouseX = Input.GetAxis("MoveCameraX") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("MoveCameraY") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);  
    }
}
