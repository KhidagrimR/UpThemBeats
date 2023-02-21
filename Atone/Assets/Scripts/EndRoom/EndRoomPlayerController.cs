using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class EndRoomPlayerController : MonoBehaviour {


    public float speedRotation;

    public void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update() {
        if(Input.GetAxis("Mouse X") != 0)
            transform.rotation = Quaternion.Euler(0, transform.rotation.y + Input.GetAxis("Mouse X") * speedRotation, 0);
    }
}
