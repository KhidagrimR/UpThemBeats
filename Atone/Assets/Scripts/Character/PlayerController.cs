using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Rigidbody rb;
    Vector3 playerVelocity;

    [Header("Status")]
    public bool isGrounded;
    public bool isChangingLane;

    [Header("Input parameters")]
    [SerializeField]
    bool canJump = false;
    [SerializeField]
    bool isAutorunActivated = true;


    [Header("Physics parameters")]
    [SerializeField]
    float _playerSpeed = 2.0f;
    [SerializeField]
    float jumpHeight = 1.0f;
    [SerializeField]
    float gravityValue = -9.81f;
    [SerializeField]
    float changeLaneDuration = 0.25f;

    private float startingPlayerY;

    #region Setter
    public float playerSpeed
    {
        get { return _playerSpeed; }
        set { _playerSpeed = value; }
    }
    #endregion

    public static GameObject gameObjectColliding;

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        startingPlayerY = transform.position.y;

        InputManager.Instance.onDestroyWall += CheckIfWallToDestroy;
        InputManager.Instance.onDestroyBop += CheckIfBopToDestroy;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.onDestroyWall -= CheckIfWallToDestroy;
            InputManager.Instance.onDestroyBop -= CheckIfBopToDestroy;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isReady) return;

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

    void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (playerSpeed * Time.deltaTime));
    }

    void Jump()
    {

    }

    void ApplyGravity()
    {
        // apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    public void ChangeLane(Vector3 lanePosition)
    {
        if (isChangingLane) return;

        isChangingLane = true;

        Vector3 target = new Vector3(lanePosition.x, lanePosition.y + startingPlayerY, transform.position.z);
        float distanceZ = playerSpeed * changeLaneDuration; //v * t

        target.z += distanceZ;

        transform.DOMove(target, changeLaneDuration).OnComplete(() =>
        {
            isChangingLane = false;
        });
    }
    public void CheckIfWallToDestroy()
    {
        if (gameObjectColliding != null)
            if (gameObjectColliding.GetComponent<WallTrigger>() != null)
                gameObjectColliding.GetComponent<WallTrigger>().WallAction();
            else
                print("coolDown - mur raté PC");
        else
            print("cooldown");
    }

    public void CheckIfBopToDestroy()
    {
        if (gameObjectColliding != null)
            if (gameObjectColliding.GetComponent<BopTrigger>() != null)
                gameObjectColliding.GetComponent<BopTrigger>().BopAction();
            else
                print("coolDown - bop raté PC");
        else
            print("cooldown");
    }
}

/*
    void Jump() // à remplacer par le saut pour changer de lane
    {
        // change the height position of the player
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
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


*/
