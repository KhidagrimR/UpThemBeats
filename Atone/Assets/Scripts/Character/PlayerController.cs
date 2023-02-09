using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Rigidbody rb;
    Vector3 playerVelocity;

    [Header("Status (can't be modified in inspector)")]
    [InspectorReadOnly] public bool isGrounded;
    [InspectorReadOnly] public bool isSliding; // is the player sliding
    [InspectorReadOnly] public bool isChangingLane; // momentum while player is moving from a lane to an other
    [InspectorReadOnly] public bool isAbleToChangeLane = false;
    [InspectorReadOnly] public bool canPlayerMove = false;

    [Header("Input parameters")]
    [SerializeField]
    bool isAutorunActivated = true;

    [Header("Player Life and Score")]
    public int initHp;
    [InspectorReadOnly]
    public static int hp;

    public float scoreMultipliyer;

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
    [SerializeField] private Animator armAnimStates;
    public Animator ArmAnimStates { get => armAnimStates; }

    #region Setter
    public float playerSpeed
    {
        get { return _playerSpeed; }
        set { _playerSpeed = value; }
    }

    internal static void IncreaseScore(float v1, object playerPositionZ, float v2, object positionBeatPerfect, int v3, object pointObstacle)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    [Header("References")]
    public GameObject playerVisual;
    public Collider playerCollider;
    public StudioEventEmitter patinDroitFMODEmitter;
    public StudioEventEmitter patinGaucheFMODEmitter;

    public static List<GameObject> gameObjectsColliding;
    public static Vector3 checkpoint;

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        startingPlayerY = transform.position.y;
        startingHeadPosition = playerCollider.transform.position;

        MusicManager.Instance.onMusicEnd += StopPlayer;
        MusicManager.Instance.onMusicStart += StartPlayer;
        MusicManager.beatUpdated += PlayPatinSounds;

        gameObjectsColliding = new List<GameObject>();
        hp = initHp;
        checkpoint = gameObject.transform.position;

        PlayerManager.scoreMultipliyer = scoreMultipliyer;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            MusicManager.Instance.onMusicEnd -= StopPlayer;
            MusicManager.Instance.onMusicStart -= StartPlayer;
            MusicManager.beatUpdated -= PlayPatinSounds;
        }

    }

    void StopPlayer()
    {
        canPlayerMove = false;
    }

    void StartPlayer()
    {
        canPlayerMove = true;
    }

    private int beat;
    void PlayPatinSounds()
    {
        beat++;
        if (beat % 2 == 0)
        {
            patinDroitFMODEmitter.Play();
        }
        else
        {
            patinGaucheFMODEmitter.Play();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isReady) return;

        //CheckGround();

        if (canPlayerMove)
            Move();

        //ApplyGravity();

        // vertical mvt
        //controller.Move(playerVelocity * Time.deltaTime);
    }

    void CheckGround()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }
    }

    // OLD WAY TO MOVE
    void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (playerSpeed * Time.deltaTime));
    }

    #region Test
    // NEW WAY TO MOVE
    /*bool isMoving;
    void Move()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (playerSpeed * Time.deltaTime));

        isMoving = true;
        Vector3 targetPosition = new Vector3(0f,0f,20f);
        float duration = MusicManager.Instance.SecPerBeat;
        // on sait qu'il commence 
        transform.DOMove(transform.position + targetPosition, duration).OnComplete(() => {
            isMoving = false;
        });
    }*/
    #endregion

    void ApplyGravity()
    {
        // apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    public void BendOnLeft()
    {
        if (PlayerManager.Instance.playerCurrentLane == 1)
        {
            transform.position = new Vector3(-0.3f, transform.position.y, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.LeanLeft);
        }
        else if (PlayerManager.Instance.playerCurrentLane == 0)
        {
            transform.position = new Vector3(-1.75f, transform.position.y, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.LeanLeft);
        }
    }
    public void ResetBend()
    {
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        animationTrigger.PlayAnimation(AnimationEnum.LeanStop);
    }
    public void BendOnRight()
    {
        if (PlayerManager.Instance.playerCurrentLane == 1)
        {
            transform.position = new Vector3(0.3f, transform.position.y, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.LeanRight);
        }
        else if (PlayerManager.Instance.playerCurrentLane == 2)
        {
            transform.position = new Vector3(1.75f, transform.position.y, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.LeanRight);
        }
    }

    public void ChangeLane(Vector3 lanePosition)
    {
        if (isChangingLane) return;

        //Debug.Log("Has changed lane");
        Vector3 target;
        if (PlayerManager.Instance.playerCurrentLane == 1)
            target = new Vector3(lanePosition.x, lanePosition.y + startingPlayerY, transform.position.z);
        else
            target = new Vector3(lanePosition.x, lanePosition.y + 0.35f, transform.position.z);

        float distanceZ = playerSpeed * changeLaneDuration; //v * t

        target.z += distanceZ;
        isChangingLane = true;

        Debug.Log("Target = " + target);

        transform.DOMove(target, changeLaneDuration).OnComplete(() =>
        {
            isChangingLane = false;
            animationTrigger.PlayAnimation(AnimationEnum.JumpStop);
        });
    }
    public void CheckIfWallToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for (int i = 0; i < gameObjectsColliding.Count; i += 1)
            {
                if (gameObjectsColliding[i].TryGetComponent<WallTrigger>(out WallTrigger wall))
                {
                    //Debug.Log("<color=green>there is a wall to destroy</color>");
                    if(wall.isDestroy == true)
                        return;

                    wall.WallAction();
                    animationTrigger.PlayAnimation(AnimationEnum.BreakLeft);
                }
                else
                    print("coolDown - mur raté PC");
            }

        else
            print("cooldown");
    }

    private int switchArmsOnBopDestroy = 0;
    public void CheckIfBopToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for (int i = 0; i < gameObjectsColliding.Count; i += 1)
            {
                if (gameObjectsColliding[i].TryGetComponent<BopTriggerDestruction>(out BopTriggerDestruction bop))
                {
                    if(bop.isDestroy == true) 
                        return;
                    
                    bop.BopAction();

                    switchArmsOnBopDestroy++;
                    if (switchArmsOnBopDestroy % 2 == 0)
                        animationTrigger.PlayAnimation(AnimationEnum.SnapLeft);
                    else
                        animationTrigger.PlayAnimation(AnimationEnum.SnapRight);
                }
                else
                    print("coolDown - bop raté PC");
            }

        else
            print("cooldown");
    }

    public void CheckIfWall3ToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for (int i = 0; i < gameObjectsColliding.Count; i += 1)
            {
                if (gameObjectsColliding[i].GetComponent<WallTrigger3>() != null)
                    gameObjectsColliding[i].GetComponent<WallTrigger3>().WallAction();
                else
                    print("coolDown - mur raté PC");
            }

        else
            print("cooldown");
    }

    public void TakeDamage()
    {
        if ((hp -= 1) == 0)
        {
            gameObject.transform.position = checkpoint;
            hp = initHp;
        }

        else
        {
            print("take damage");
            //print("new HP : " + hp);
        }

    }



    private Vector3 startingHeadPosition;
    [Header("Slide")]
    public float headYMovement = 0.5f;
    public float headYMovementTween = 0.5f;
    public void Slide(bool pIsSliding)
    {
        //Debug.Log("Slide Called on : " + pIsSliding);
        if (pIsSliding)
        {
            // se pencher / glisser 
            // baisser la tête
            //playerCollider.transform.position = new Vector3(playerCollider.transform.position.x, headYMovement, playerCollider.transform.position.z);

            DOVirtual.Float(playerCollider.transform.position.x, headYMovement, headYMovementTween, (float x) =>
            {
                playerCollider.transform.position = new Vector3(playerCollider.transform.position.x, startingHeadPosition.y - x, playerCollider.transform.position.z);
            });

            animationTrigger.PlayAnimation(AnimationEnum.SlideStart);

            isSliding = true;
        }
        else
        {
            // se lever
            // lever la tête
            DOVirtual.Float(playerCollider.transform.position.y, startingHeadPosition.y, headYMovementTween, (float x) =>
            {
                playerCollider.transform.position = new Vector3(playerCollider.transform.position.x, x, playerCollider.transform.position.z);
            });

            animationTrigger.PlayAnimation(AnimationEnum.SlideStop);

            isSliding = false;
        }
    }
}
