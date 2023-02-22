using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
using Cinemachine;

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
    public GameObject sages;
    public Collider playerCollider;
    public StudioEventEmitter patinDroitFMODEmitter;
    public StudioEventEmitter patinGaucheFMODEmitter;
    public StudioEventEmitter playerJumpSFX;
    public StudioEventEmitter playerLandSFX;
    public StudioEventEmitter playerSlideSFX;
    public StudioEventEmitter playerWallrunLSFX;
    public StudioEventEmitter playerWallrunRSFX;
    public StudioEventEmitter playerHitSFX;
    public StudioEventEmitter playerDeathSFX;
    public StudioEventEmitter playerSnapSFX;

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
            float from = transform.position.x;
            float to = -0.3f;
            float duration = 0.25f;
            DOVirtual.Float(from, to, duration, (float x) => {
                 transform.position = new Vector3(x, transform.position.y, transform.position.z);
            });

            //transform.position = new Vector3(-0.3f, transform.position.y, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.LeanLeft);
        }
    }
    public void ResetBend()
    {
        float from = transform.position.x;
        float to = 0.0f;
        float duration = 0.25f;
        DOVirtual.Float(from, to, duration, (float x) => {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        });
        //transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        animationTrigger.PlayAnimation(AnimationEnum.LeanStop);
    }
    public void BendOnRight()
    {
        if (PlayerManager.Instance.playerCurrentLane == 1)
        {
            float from = transform.position.x;
            float to = 0.3f;
            float duration = 0.25f;
            DOVirtual.Float(from, to, duration, (float x) => {
                 transform.position = new Vector3(x, transform.position.y, transform.position.z);
            });
            //transform.position = new Vector3(0.3f, transform.position.y, transform.position.z);
            animationTrigger.PlayAnimation(AnimationEnum.LeanRight);
        }
    }

    public void ChangeLane(Vector3 lanePosition)
    {
            //if (isChangingLane) return;
        SequenceManager.Instance.currentSequence.currentAmountOFObstacleDestroyed++;
        isChangingLane = true;

        if(PlayerManager.Instance.playerCurrentLane == 0)
        {
            animationTrigger.animator.SetTrigger(AnimationEnum.TriggerLeftJump.ToString());
        }
        else if(PlayerManager.Instance.playerCurrentLane == 2)
        {
            animationTrigger.animator.SetTrigger(AnimationEnum.TriggerRightJump.ToString());
        }   
        
        // Changer l'anim du perso
        animationTrigger.PlayAnimation(AnimationEnum.JumpStart, false);
        // déclencher le son de saut
        playerJumpSFX.Play();

        // Target lane position
        Vector3 target;

        // Dans le cas de la lane centrale
        if (PlayerManager.Instance.playerCurrentLane == 1)
        {
            // on choisit de placer le joueur sur le bon endroit
            target = new Vector3(lanePosition.x, lanePosition.y + startingPlayerY, transform.position.z);
            // on met le head bob en mode classic
            CameraManager.Instance.ChangeHeadbobType(CameraManager.HeadbobNoiseSettings.HeadbobType.UpstandBob);

            // On enleve les sons de wall runs et on met les sons de déplacement classiques
            playerLandSFX.Play();
            playerWallrunLSFX.Stop();
            playerWallrunRSFX.Stop();

            // on reset le bloom (post process)
            PostProcessManager.Instance.ResetBloomColor(0.5f);

            // l'anim de stop jump du joueur se lance
            animationTrigger.PlayAnimation(AnimationEnum.JumpStop);
        }
        else
        {
            // Ajouter effet de post process
            PostProcessManager.Instance.ChangeColorToBlue(0.5f);
            //Changer le Headbob en mode wall run
            CameraManager.Instance.ChangeHeadbobType(CameraManager.HeadbobNoiseSettings.HeadbobType.WallrunBob);

            // on set la position de la piste sur laquelle on veut sauter
            target = new Vector3(lanePosition.x, lanePosition.y + 0.35f, transform.position.z);
            if (PlayerManager.Instance.playerCurrentLane == 2)
            {
                playerWallrunRSFX.Play();
            }
            else
            {
                playerWallrunLSFX.Play();
            }
        }
        // la distance à prendre en compte pour pas se décaler du beat
        float distanceZ = playerSpeed * changeLaneDuration; //v * t
        target.z += distanceZ;

        // on tween
        transform.DOMove(target, changeLaneDuration).OnComplete(() =>
        {
            // quand le tween est fini, on shake la camera
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
                    PostProcessManager.Instance.ChangeColorToRed(.2f);
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
                if(gameObjectsColliding[i] != null)
                {
                    if (gameObjectsColliding[i].TryGetComponent<BopTriggerDestruction>(out BopTriggerDestruction bop))
                    {
                        if (bop.isDestroy == true)
                            return;

                        bop.BopAction();

                        switchArmsState++;
                        playerSnapSFX.Play();
                        PostProcessManager.Instance.ChangeColorToYellow(0.2f);
                        if (switchArmsState % 2 == 0)
                            animationTrigger.PlayAnimation(AnimationEnum.SnapLeft);
                        else
                            animationTrigger.PlayAnimation(AnimationEnum.SnapRight);
                    }
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
        playerHitSFX.Play();
        if (hp <= 0)
        {
            hp = initHp;
            playerDeathSFX.Play();
            StartCoroutine(SequenceManager.Instance.RestartCurrentSequence());
            animationTrigger.PlayAnimation(AnimationEnum.Death);
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Damage);
            PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Hit);
        }
        else
        {
            //print("take damage");
            //print("new HP : " + hp);
            animationTrigger.PlayAnimation(AnimationEnum.HitTaken);
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Damage);
            PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Hit);
        }

    }

    private Vector3 startingHeadPosition;
    [Header("Slide")]
    public float headYMovement = 0.5f;
    public float headYMovementTween = 0.5f;
    public void Slide(bool pIsSliding)
    {
        animationTrigger.animator.SetBool("isSliding",pIsSliding);

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

            playerSlideSFX.Play();
            animationTrigger.PlayAnimation(AnimationEnum.SlideStart);
            isSliding = true;
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Slide);
            CameraManager.Instance.ChangeHeadbobType(CameraManager.HeadbobNoiseSettings.HeadbobType.SlideBob);
            PostProcessManager.Instance.ChangeColorToBlue(0.5f);
            PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Slide);
            //vcam.m_Lens.FieldOfView = 1;
            //Debug.Log("<color=orange>Vcam = "+vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain+"</color>");
        }
        else
        {
            // se lever
            // lever la tête
            DOVirtual.Float(playerCollider.transform.position.y, startingHeadPosition.y, headYMovementTween, (float x) =>
            {
                playerCollider.transform.position = new Vector3(playerCollider.transform.position.x, x, playerCollider.transform.position.z);
            });

            playerSlideSFX.Stop();
            animationTrigger.PlayAnimation(AnimationEnum.SlideStop);
            isSliding = false;
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.SlideStop);
            CameraManager.Instance.ChangeHeadbobType(CameraManager.HeadbobNoiseSettings.HeadbobType.UpstandBob);
            PostProcessManager.Instance.ResetBloomColor(.5f);
            PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Standing);
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
