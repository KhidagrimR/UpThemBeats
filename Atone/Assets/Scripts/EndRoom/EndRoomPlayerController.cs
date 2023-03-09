using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EndRoomPlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Animator animator;
 public Animator cameraAnimator;
    public float speed = 12f;

    public float gravity = -9.81f;

    public float jumpHeigt = 3f;

    public Camera playerCam;

    public Transform groundCheck;
    public float groundRadiusCheck = 0.4f;
    public LayerMask layerMask;

    public static bool canMove = false;
    public static bool inAnimationFinalDoor = false;
    public static bool isWalking = false;
    public static bool playingSfx = false;

    public List<StudioEventEmitter> walk;
    public bool isRightWalk = false;


    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame

    public void Start() {
        //animator.SetTrigger("wakeUp");
    }
    void Update()
    {
        if (canMove){
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadiusCheck, layerMask);

            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                isWalking = true;
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = playerCam.transform.right * x + playerCam.transform.forward * z;

                characterController.Move(move * speed * Time.deltaTime);

                velocity.y += gravity * Time.deltaTime;

                characterController.Move(velocity * Time.deltaTime);
            }
            else
                isWalking = false;

            if (!isWalking)
                playingSfx = false;

            if(!playingSfx)
                StartCoroutine(SFXManager());
        }
        if (inAnimationFinalDoor){
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.O) ||
                Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton3))
                gameObject.GetComponent<DisplayScoreEndingRoom>().DisplayInputName();
        }
        
    }

    public IEnumerator SFXManager() {
        if (isWalking){
            playingSfx = true;
            if (isRightWalk)
                walk[1].Play();
            else
                walk[0].Play();
            yield return new WaitForSeconds(0.5f);
            isRightWalk = !isRightWalk;
            playingSfx = false;
        }
        
        
    }
}
