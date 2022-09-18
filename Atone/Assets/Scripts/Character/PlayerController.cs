using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;

    bool isGrounded;

    [Header("Physics parameters")]
    [SerializeField]
    float playerSpeed = 2.0f;
    [SerializeField]
    float jumpHeight = 1.0f;
    [SerializeField]
    float gravityValue = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // change the height position of the player
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
