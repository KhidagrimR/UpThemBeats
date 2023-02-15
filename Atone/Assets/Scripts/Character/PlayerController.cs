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
    [InspectorReadOnly] public bool _isSliding; // is the player sliding
    [InspectorReadOnly] public bool isIndestructible; // is the player sliding
    public bool isSliding
    {
        get { return _isSliding; }
        set
        {

            // code for trigger event here
            // trigger once on true
            if (_isSliding != value)
            {
                if (value == true)
                {
                    playerAnimationEvent.VFXSlideTrigger();
                }
                else
                {
                    playerAnimationEvent.VFXSlideStopTrigger();
                }
            }
            _isSliding = value;
        }
    }
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
    public Vector3 currentCheckpoint;
    public PlayerAnimationEvent playerAnimationEvent;
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
        currentCheckpoint = gameObject.transform.position;

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
        if(!isSliding && PlayerManager.Instance.playerCurrentLane == 1)
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
    }

    public void ChangeLane(Vector3 lanePosition)
    {
        if (isChangingLane) return;
        animationTrigger.PlayAnimation(AnimationEnum.JumpStart);
        Vector3 target;
        if (PlayerManager.Instance.playerCurrentLane == 1)
        {
            target = new Vector3(lanePosition.x, lanePosition.y + startingPlayerY, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.JumpStop);
        }
        else
            target = new Vector3(lanePosition.x, lanePosition.y + 0.35f, transform.position.z);

        float distanceZ = playerSpeed * changeLaneDuration; //v * t

        target.z += distanceZ;
        isChangingLane = true;

        //Debug.Log("Target = " + target);

        transform.DOMove(target, changeLaneDuration).OnComplete(() =>
        {
            isChangingLane = false;
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.WallrunHit);

        });
    }

    private int switchArmsState = 0; // for bop and red wall

    public void CheckIfWallToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for (int i = 0; i < gameObjectsColliding.Count; i += 1)
            {
                if (gameObjectsColliding[i].TryGetComponent<WallTrigger>(out WallTrigger wall))
                {
                    //Debug.Log("<color=green>there is a wall to destroy</color>");
                    if (wall.isDestroy == true)
                        return;

                    wall.WallAction();
                    switchArmsState++;
                    if (switchArmsState % 2 == 0)
                        animationTrigger.PlayAnimation(AnimationEnum.BreakLeft);
                    else
                        animationTrigger.PlayAnimation(AnimationEnum.BreakRight);
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
                    if (bop.isDestroy == true)
                        return;

                    bop.BopAction();

                    switchArmsState++;
                    if (switchArmsState % 2 == 0)
                        animationTrigger.PlayAnimation(AnimationEnum.SnapLeft);
                    else
                        animationTrigger.PlayAnimation(AnimationEnum.SnapRight);
                }
                /*else
                    print("coolDown - bop raté PC");*/
            }

        /*else
            print("cooldown");*/
    }

    public void CheckIfWall3ToDestroy()
    {
        if (gameObjectsColliding.Count != 0)
            for (int i = 0; i < gameObjectsColliding.Count; i += 1)
            {
                if (gameObjectsColliding[i].GetComponent<WallTrigger3>() != null)
                    gameObjectsColliding[i].GetComponent<WallTrigger3>().WallAction();
                /*else
                    print("coolDown - mur raté PC");*/
            }

        /*else
            print("cooldown");*/
    }

    public void TakeDamage()
    {
        if(isIndestructible) return;
        StartCoroutine(SetIndestructible());
        hp --;
        if (hp <= 0)
        {
           
            hp = initHp;
            StartCoroutine(SequenceManager.Instance.RestartCurrentSequence());
            animationTrigger.PlayAnimation(AnimationEnum.Death);
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Damage);
        }
        else
        {
            print("take damage");
            //print("new HP : " + hp);
            animationTrigger.PlayAnimation(AnimationEnum.HitTaken);
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Damage);
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
            if (isSliding) return;

            DOVirtual.Float(playerCollider.transform.position.x, headYMovement, headYMovementTween, (float x) =>
            {
                playerCollider.transform.position = new Vector3(playerCollider.transform.position.x, startingHeadPosition.y - x, playerCollider.transform.position.z);
            });

            animationTrigger.PlayAnimation(AnimationEnum.SlideStart);
            isSliding = true;
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Slide);
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
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.SlideStop);
        }
    }

    public IEnumerator CheckIfPlayerCanStayOnWall()
    {
        while(PlayerManager.Instance.playerCurrentLane != 1)
        {
            
            yield return new WaitForSeconds(0.3f);
            bool canPlayerStillWallRun = true;

            if(PlayerManager.Instance.playerCurrentLane == 0)
            {
                //Debug.Log("<color=green>CHECK</color>");
                canPlayerStillWallRun = PlayerManager.Instance.CheckIfCanStayOnWall(-1);
                if(canPlayerStillWallRun == false)
                {
                    PlayerManager.Instance.MovePlayerToRightLane();
                    PlayerManager.Instance.BendPlayerTowardDirection(1);
                }
            }
            else if(PlayerManager.Instance.playerCurrentLane == 2)
            {
                canPlayerStillWallRun = PlayerManager.Instance.CheckIfCanStayOnWall(1);
                if(canPlayerStillWallRun == false)
                {
                    PlayerManager.Instance.MovePlayerToLeftLane();
                    PlayerManager.Instance.BendPlayerTowardDirection(1);
                }
            }
                

            // check if player has a wall on left and on right
        }
    }

    public IEnumerator SetIndestructible()
    {
        isIndestructible = true;
        yield return new WaitForSeconds(0.5f);
        isIndestructible = false;
    }
}
