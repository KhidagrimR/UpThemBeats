using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;

public class PlayerManager : Singleton<PlayerManager>
{
    public static string PLAYER_TAG = "Player";
    public PlayerController playerController;
    public Transform playerHead;

    public PivotPointAlignment rootPivot; // "Root" parce qu'il n'est pas censé être enfant d'un autre objet

    public CinemachineVirtualCamera cvm;
    public LayerMask laneLayerMask;


    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }

    #region getter on player status
    public bool isPlayerAbleToChangeLane
    {
        get
        {
            // le joueur peut changer LIBREMENT de Lane s'il n'est pas déjà en train de changer de lane OU s'il n'est pas sur la lane centrale
            return ((playerController.isAbleToChangeLane || playerCurrentLane != 1) && !playerController.isSliding); // 1 => center lane
        }
    }
    public bool isPlayerAbleToSlide
    {
        get
        {
            // le joueur peut Slider tant qu'il n'est pas en train de courir sur un mur et en train de changer de lane
            return (!playerController.isChangingLane && playerCurrentLane == 1); // 1 => center lane
        }
    }

    public bool isPlayerAbleToDestroyObstacles
    {
        get
        {
            // le joueur peut détruire un obstacle s' il, n'est pas en train de slider ou de wall run
            return (!playerController.isChangingLane && playerCurrentLane == 1 && !playerController.isSliding); // 1 => center lane
        }
    }

    #endregion

    public Transform[] lanes;

    [InspectorReadOnly]
    public int playerCurrentLane = 1;


    [Header("To Tweak")]
    public float tweenDutchDuration = 0.4f;

    [InspectorReadOnly]
    public static float scoreSequence;
    public static float scoreMultipliyer;

    public Dictionary<string, Dictionary<string, float>> scoreBoard;

    [HideInInspector] public ChangeLaneTrigger gameObjectTriggerChangeLane;

    public void Init()
    {
        SetupPlayerSpeed();
        SetupPlayerAnimationSpeed();
        InitDistanceBop();

        InputManager.Instance.onGoLeftLane += MovePlayerToLeftLane;
        InputManager.Instance.onGoRightLane += MovePlayerToRightLane;

        InputManager.Instance.onBendLane += BendPlayerTowardDirection;
        InputManager.Instance.onBendReleaseLane += ReleasePlayerArmAnimation;

        InputManager.Instance.onDestroyWall += MakePlayerDestroyWall;
        InputManager.Instance.onDestroyBop += MakePlayerDestroyBop;

        InputManager.Instance.onSlide += MakePlayerSlide;

        playerCurrentLane = 1;
        scoreSequence = 0;
        scoreBoard = new Dictionary<string, Dictionary<string, float>>();
        _isReady = true;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.onGoLeftLane -= MovePlayerToLeftLane;
            InputManager.Instance.onGoRightLane -= MovePlayerToRightLane;

            InputManager.Instance.onSlide -= MakePlayerSlide;

            InputManager.Instance.onDestroyWall -= MakePlayerDestroyWall;
            InputManager.Instance.onDestroyBop -= MakePlayerDestroyBop;

            InputManager.Instance.onBendLane -= BendPlayerTowardDirection;
            InputManager.Instance.onBendReleaseLane -= ReleasePlayerArmAnimation;
        }
    }

    void SetupPlayerSpeed()
    {
        float playerSpeed = 1f;
        float distanceBetweenBeats = SoundCreator.Instance.DistanceBetweenNotes;
        float secPerBeat = SoundCreator.Instance.SecPerBeat;

        // here you calculate the speed to reach the next "beat" point
        // v = d / t => d = distanceBetweenBeats and t => secPerBeat
        playerSpeed = distanceBetweenBeats / secPerBeat;

        playerController.playerSpeed = playerSpeed;

        if (rootPivot)
        {
            rootPivot.pSpeed = playerSpeed;
        }
    }

    void SetupPlayerAnimationSpeed()
    {
        // just a reminder to modify the player animation speed to make it goes up and down on the beats
        // Debug.Log("both arms index: " +playerController.ArmAnimStates.GetLayerIndex("Both Arms")); // It's 0
        // AnimatorStateInfo skateState = playerController.ArmAnimStates.GetCurrentAnimatorStateInfo(0);
        // Debug.Log("clip length ? "+ playerController.ArmAnimStates.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // We know it's 80 bpm, so screw it, I'm hard coding this.
        float clipLength = playerController.ArmAnimStates.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        // Arm peak on half beat (160), cycle on full beat. 60/160 = 0.375
        float cycleSpeedScale = 0.375f * 1f / ((0.91f - 0.39f) * clipLength);
        // normalised cycle peaks : 0.39 - 0.91 // J'ai fair ça au coup d'oeil, rien de super précis
        playerController.ArmAnimStates.SetFloat(GameAnimatorsParams.AnimParamsDict[AnimationEnum.SkateCycleOffset], 0.39f);
        playerController.ArmAnimStates.SetFloat(GameAnimatorsParams.AnimParamsDict[AnimationEnum.SpeedScaleFactor], cycleSpeedScale);
        // delete the function if that's already done as you read this
    }

    public void MovePlayerToRightLane()
    {
        if (isPlayerAbleToChangeLane && CheckIfCanSwitchLane(1))
        {
            playerCurrentLane++;
            ChangeLaneDutch(playerCurrentLane);

            if (gameObjectTriggerChangeLane != null)
                IncreaseScore(gameObjectTriggerChangeLane.GetComponent<BoxCollider>().bounds.extents.z, gameObjectTriggerChangeLane.transform.position.z, gameObjectTriggerChangeLane.pointObstacle);

            if (playerCurrentLane != 1)
            {
                playerController.animationTrigger.PlayAnimation(AnimationEnum.JumpStart);
                StartCoroutine(playerController.CheckIfPlayerCanStayOnWall());
            }
            else
            {
                StopCoroutine(playerController.CheckIfPlayerCanStayOnWall());
            }
                

            playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
            playerController.ChangeLane(GetLanePosition(playerCurrentLane));
        }
    }

    public void MovePlayerToLeftLane()
    {
        if (isPlayerAbleToChangeLane && CheckIfCanSwitchLane(-1))
        {
            playerCurrentLane--;
            ChangeLaneDutch(playerCurrentLane);

            if (gameObjectTriggerChangeLane != null)
                IncreaseScore(gameObjectTriggerChangeLane.GetComponent<BoxCollider>().bounds.extents.z, gameObjectTriggerChangeLane.transform.position.z, gameObjectTriggerChangeLane.pointObstacle);

            if (playerCurrentLane != 1)
            {
                playerController.animationTrigger.PlayAnimation(AnimationEnum.JumpStart);
                StartCoroutine(playerController.CheckIfPlayerCanStayOnWall());
            }
            else
            {
                StopCoroutine(playerController.CheckIfPlayerCanStayOnWall());
            }
                

            playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
            playerController.ChangeLane(GetLanePosition(playerCurrentLane));
        }
    }

    public bool CheckIfCanSwitchLane(int direction)
    {
        //Debug.Log("CHECK LANE SWITCH : direction = " + direction);
        direction = direction > 0 ? 1 : -1;
        // si on est sur la ligne du centre, on veut voir si on peut aller sur une lane latérale
        if (PlayerManager.Instance.playerCurrentLane == 1)
        {
            // on tire un rayon en direction de la direction 
            RaycastHit hit;
            Vector3 playerPos = playerController.transform.position;

            if (Physics.Raycast(playerPos, Vector3.right * direction, out hit, Mathf.Infinity, laneLayerMask))
            {
                //Debug.Log("Hit = " + hit.collider.gameObject.name);
                return true;
            }
            else
            {
                //Debug.Log("NO RAYCAST HIT");
                return false;
            }
        }
        else
            return true;
    }

    public bool CheckIfCanStayOnWall(int direction)
    {
        //Debug.Log("CHECK LANE SWITCH : direction = " + direction);
        direction = direction > 0 ? 1 : -1;
        // si on est sur un mur, on veut savoir si on est tjr sur le mur
        if (PlayerManager.Instance.playerCurrentLane != 1)
        {
            // on tire un rayon en direction de la direction 
            RaycastHit hit;
            Vector3 playerPos = playerController.transform.position;

            if (Physics.Raycast(playerPos, Vector3.right * direction, out hit, Mathf.Infinity, laneLayerMask))
            {
                //Debug.Log("Hit = " + hit.collider.gameObject.name);
                return true;
            }
            else
            {
                //Debug.Log("NO RAYCAST HIT");
                return false;
            }
        }
        else
            return true;
    }

    public void ReleasePlayerArmAnimation(int direction)
    {
        /*
        if (direction == 0)
        {
            playerController.BendOnRight();
        }

        if (direction == 2)
        {
            playerController.BendOnLeft();
        }
        */
    }

    public void MakePlayerDestroyWall()
    {
        if (isPlayerAbleToDestroyObstacles)
        {
            playerController.CheckIfWallToDestroy();
        }
    }

    public void MakePlayerDestroyBop()
    {
        if (isPlayerAbleToDestroyObstacles)
            playerController.CheckIfBopToDestroy();
    }


    public void MakePlayerSlide(bool pIsSliding)
    {
        // if the player is NOT sliding but the button is released (means that we want the player to stand up while he is already)
        // it may trigger the animator when we don't want
        // disallow that action instead
        if (pIsSliding == false && !playerController.isSliding)
            return;

        if (isPlayerAbleToSlide)
        {
            playerController.Slide(pIsSliding);
            //StartCoroutine(CameraManager.Instance.ShakeCameraWhileSliding());
        }
    }

    public void BendPlayerTowardDirection(int direction)
    {
        //if (playerCurrentLane != 1) return;

        //Debug.Log("Bend on : "+direction);
        switch (direction)
        {
            case 0:
                playerController.BendOnLeft();
                DOVirtual.Float(cvm.m_Lens.Dutch, -10f, tweenDutchDuration / 2f, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                break;
            case 1:
                playerController.ResetBend();
                DOVirtual.Float(cvm.m_Lens.Dutch, 0f, tweenDutchDuration / 2f, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });

                if (playerCurrentLane <= 0) MovePlayerToRightLane();
                else if (playerCurrentLane >= 2) MovePlayerToLeftLane();
                break;
            case 2:
                playerController.BendOnRight();
                DOVirtual.Float(cvm.m_Lens.Dutch, 10f, tweenDutchDuration / 2f, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                break;
        }
    }

    private void ChangeLaneDutch(int lane)
    {
        //        Debug.Log("CALLED");
        switch (lane)
        {
            case 0: // Gauche
                DOVirtual.Float(cvm.m_Lens.Dutch, -20f, tweenDutchDuration, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                //cvm.m_Lens.Dutch = -20;
                playerHead.localPosition = new Vector3(1, playerHead.localPosition.y, playerHead.localPosition.z);
                //playerController.animationTrigger.PlayAnimation(AnimationEnum.LeftRun);
                break;

            case 1: // centre
                //cvm.m_Lens.Dutch = 0;
                DOVirtual.Float(cvm.m_Lens.Dutch, 0f, tweenDutchDuration, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                playerHead.localPosition = new Vector3(0, playerHead.localPosition.y, playerHead.localPosition.z);
                //playerController.animationTrigger.PlayAnimation(AnimationEnum.Run);
                break;

            case 2: // droite
                //cvm.m_Lens.Dutch = 20;
                DOVirtual.Float(cvm.m_Lens.Dutch, 20f, tweenDutchDuration, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                playerHead.localPosition = new Vector3(-1, playerHead.localPosition.y, playerHead.localPosition.z);
                //playerController.animationTrigger.PlayAnimation(AnimationEnum.RightRun);
                break;
        }

    }

    public Vector3 GetLanePosition(int targetLane)
    {
        //Debug.Log("<color=green>go to lane "+targetLane+"</color>");
        //Debug.Log("<color=green>lane position = "+lanes[targetLane].position+"</color>");
        return lanes[targetLane].position;
    }

    public void InitDistanceBop()
    {
        //print("InitBop");
        foreach (GameObject bop in GameObject.FindGameObjectsWithTag("Bop"))
        {
            bop.GetComponentInChildren<BopTriggerArrival>().InitDistance();
        }

    }

    public void IncreaseScore(float boundZCollider, float positionBeatPerfect, int pointObstacle)
    {
        //Debug.Log("positionBeatPerfectZ : " + positionBeatPerfect);
        //Debug.Log("ScoreMultipliyer : " + (Math.Abs(positionBeatPerfect - playerController.transform.position.z) / boundZCollider / 2) / 2);

        float distanceToCenter = Math.Abs(positionBeatPerfect - playerController.transform.position.z);
        if (distanceToCenter > boundZCollider / 2)
            scoreSequence += scoreMultipliyer * pointObstacle;
        else
            scoreSequence += (scoreMultipliyer + (scoreMultipliyer / 2)) * pointObstacle;
        scoreSequence = (float)Math.Round(scoreSequence, 1);
        //Debug.Log("nouveau score = " + scoreSequence);
    }
}
