using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerManager : Singleton<PlayerManager>
{
    public static string PLAYER_TAG = "Player";
    public PlayerController playerController;
    public Transform playerHead;

    public PivotPointAlignment rootPivot; // "Root" parce qu'il n'est pas censé être enfant d'un autre objet

    public CinemachineVirtualCamera cvm;

    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }

    public bool isPlayerAbleToChangeLane
    {
        get
        {
            return (playerController.isAbleToChangeLane || playerCurrentLane != 1); // 1 => center lane
        }
    }

    public Transform[] lanes;

    [InspectorReadOnly]
    public int playerCurrentLane = 1;

    [Header("To Tweak")]
    public float tweenDutchDuration = 0.4f;

    public void Init()
    {
        SetupPlayerSpeed();
        InitDistanceBop();

        InputManager.Instance.onGoLeftLane += MovePlayerToLeftLane;
        InputManager.Instance.onGoRightLane += MovePlayerToRightLane;

        InputManager.Instance.onBendLane += BendPlayerTowardDirection;
        InputManager.Instance.onBendReleaseLane += ReleasePlayerArmAnimation;

        _isReady = true;
        playerCurrentLane = 1;

    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.onGoLeftLane -= MovePlayerToLeftLane;
            InputManager.Instance.onGoRightLane -= MovePlayerToRightLane;

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

        // delete the function if that's already done as you read this
    }

    public void MovePlayerToRightLane()
    {
        if (isPlayerAbleToChangeLane)
        {
            playerCurrentLane++;
            ChangeLaneDutch(playerCurrentLane);

            if(playerCurrentLane != 1)
                playerController.animationTrigger.PlayAnimation(AnimationEnum.Jump);

            playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
            playerController.ChangeLane(GetLanePosition(playerCurrentLane));
        }
    }

    public void MovePlayerToLeftLane()
    {
        if (isPlayerAbleToChangeLane)
        {
            playerCurrentLane--;
            ChangeLaneDutch(playerCurrentLane);

            if(playerCurrentLane != 1)
                playerController.animationTrigger.PlayAnimation(AnimationEnum.Jump);

            playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
            playerController.ChangeLane(GetLanePosition(playerCurrentLane));
        }
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

    public void BendPlayerTowardDirection(int direction)
    {
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
                playerController.animationTrigger.PlayAnimation(AnimationEnum.LeftRun);    // AnimationTrigger.AnimationEnum.LeftRun
                break;

            case 1: // centre
                //cvm.m_Lens.Dutch = 0;
                DOVirtual.Float(cvm.m_Lens.Dutch, 0f, tweenDutchDuration, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                playerHead.localPosition = new Vector3(0, playerHead.localPosition.y, playerHead.localPosition.z);
                playerController.animationTrigger.PlayAnimation(AnimationEnum.Run);
                break;

            case 2: // droite
                //cvm.m_Lens.Dutch = 20;
                DOVirtual.Float(cvm.m_Lens.Dutch, 20f, tweenDutchDuration, (float x) =>
                {
                    cvm.m_Lens.Dutch = x;
                });
                playerHead.localPosition = new Vector3(-1, playerHead.localPosition.y, playerHead.localPosition.z);
                playerController.animationTrigger.PlayAnimation(AnimationEnum.RightRun);
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
        print("InitBop");
        foreach (GameObject bop in GameObject.FindGameObjectsWithTag("Bop"))
        {
            bop.GetComponentInChildren<BopTriggerArrival>().InitDistance();
        }

    }
}
