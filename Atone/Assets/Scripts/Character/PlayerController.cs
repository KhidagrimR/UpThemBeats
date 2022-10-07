using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;

    [Header("Status")]
    public bool isGrounded;

    [Header("Input parameters")]
    [SerializeField]
    bool canJump = false;
    [SerializeField]
    bool isAutorunActivated = true;


    [Header("Physics parameters")]
    [SerializeField]
    float playerSpeed = 2.0f;
    [SerializeField]
    float jumpHeight = 1.0f;
    [SerializeField]
    float gravityValue = -9.81f;

    // Var for fast acceleration
    [Header("Acceleration params")]
    private float accelerationValue = 20;
    private float currentAccelerationValue;
    private float decelerationFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();

        Move();

        if (canJump && isGrounded && Input.GetButtonDown("Jump"))
            Jump();

        ApplyGravity();

        // vertical mvt
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void CheckGround()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }
    }

    void Move() // peut être à refaire pour qu'on ait une vitesse en BPM ?
    {
        Vector3 move;
        if (isAutorunActivated)
            move = new Vector3(0, 0, 1);
        else
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        controller.Move(move * Time.deltaTime * playerSpeed);
    }

    void Jump() // à remplacer par le saut pour changer de lane
    {
        // change the height position of the player
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    void ApplyGravity()
    {
        // apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
    }
}
