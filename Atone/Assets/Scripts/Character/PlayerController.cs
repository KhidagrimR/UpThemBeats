using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Rigidbody rb;
    Vector3 playerVelocity;

    [Header("Status (can't be modified in inspector)")]
    [InspectorReadOnly]
    public bool isGrounded;
    public int initHp;
    [InspectorReadOnly]
    public static int hp;

    [InspectorReadOnly]
    public bool isChangingLane;
    [InspectorReadOnly]
    public bool isAbleToChangeLane = false;

    [Header("Input parameters")]
    [SerializeField]
    bool isAutorunActivated = true;


    [Header("Physics parameters")]
    [SerializeField]
    [InspectorReadOnly]
    float _playerSpeed = 2.0f;
    [SerializeField]
    float gravityValue = -9.81f;
    [SerializeField]
    float changeLaneDuration = 0.25f;

    private float startingPlayerY;

    public AnimationTrigger animationTrigger;

    #region Setter
    public float playerSpeed
    {
        get { return _playerSpeed; }
        set { _playerSpeed = value; }
    }
    #endregion

    [Header("References")]
    public GameObject playerVisual;

    public static List<GameObject> gameObjectsColliding;
    public static Vector3 checkpoint;

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        startingPlayerY = transform.position.y;

        InputManager.Instance.onDestroyWall += CheckIfWallToDestroy;
        InputManager.Instance.onDestroyBop += CheckIfBopToDestroy;

        gameObjectsColliding = new List<GameObject>();
        hp = initHp;
        checkpoint = gameObject.transform.position;
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

    void ApplyGravity()
    {
        // apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    public void BendOnLeft()
    {
        transform.position = new Vector3(-0.3f, transform.position.y, transform.position.z);
        animationTrigger.PlayAnimation(AnimationEnum.LeanLeft);
    }
    public void ResetBend()
    {
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        animationTrigger.PlayAnimation(AnimationEnum.LeanStop);
    }
    public void BendOnRight()
    {
        transform.position = new Vector3(0.3f, transform.position.y, transform.position.z);
        animationTrigger.PlayAnimation(AnimationEnum.LeanRight);
    }

    public void ChangeLane(Vector3 lanePosition)
    {
        if (isChangingLane) return;

        print("Has changed lane");

        isChangingLane = true;

        Vector3 target = new Vector3(lanePosition.x, lanePosition.y + startingPlayerY, transform.position.z);
        float distanceZ = playerSpeed * changeLaneDuration; //v * t

        target.z += distanceZ;

        transform.DOMove(target, changeLaneDuration).OnComplete(() =>
        {
            isChangingLane = false;
            animationTrigger.PlayAnimation(AnimationEnum.JumpStop);
        });
    }
    public void CheckIfWallToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for(int i = 0; i < gameObjectsColliding.Count; i+= 1){
                if (gameObjectsColliding[i].GetComponent<WallTrigger>() != null)
                {
                        gameObjectsColliding[i].GetComponent<WallTrigger>().WallAction();
                        animationTrigger.PlayAnimation(AnimationEnum.Break);
                }
                    else
                        print("coolDown - mur raté PC");
            }
                
        else
            print("cooldown");
    }

    public void CheckIfBopToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for (int i = 0; i < gameObjectsColliding.Count; i += 1)
            {
                if (gameObjectsColliding[i].TryGetComponent<BopTriggerDestruction>(out BopTriggerDestruction bop))
                {
                    bop.BopAction();

                    int rd = Random.Range(0, 1);
                    if(rd == 0)
                        animationTrigger.PlayAnimation(AnimationEnum.LeftSnap);
                    else
                        animationTrigger.PlayAnimation(AnimationEnum.LeftSnap);
                }
                else
                    print("coolDown - bop raté PC");
            }

        else
            print("cooldown");
    }

    public void TakeDamage(){
        if ((hp -= 1) == 0){
            gameObject.transform.position = checkpoint;
            hp = initHp;
        }
            
        else{
            print("take damage");
            print("new HP : " + hp);
        }
            
    }
}
