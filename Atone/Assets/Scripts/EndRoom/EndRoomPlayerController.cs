using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomPlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Animator animator;

    public float speed = 12f;

    public float gravity = -9.81f;

    public float jumpHeigt = 3f;

    public Transform groundCheck;
    public float groundRadiusCheck = 0.4f;
    public LayerMask layerMask;

    public static bool canMove = false;
    public static bool inAnimationFinalDoor = false;


    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame

    public void Start() {
        animator.SetTrigger("wakeUp");
    }
    void Update()
    {
        if (canMove){
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadiusCheck, layerMask);

            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;


            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            characterController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeigt * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);
        }
        if (inAnimationFinalDoor){
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.O) ||
               Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton3))
                gameObject.GetComponent<DisplayScoreEndingRoom>().DisplayInputName();
        }
        
    }
}
