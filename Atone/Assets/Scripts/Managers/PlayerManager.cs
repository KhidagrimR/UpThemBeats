using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    public Transform[] lanes;

    public int playerCurrentLane = 1;


    public void Init()
    {
        SetupPlayerSpeed();
        InitDistanceBop();

        InputManager.Instance.onGoLeftLanePressed += MovePlayerToLeftLane;
        InputManager.Instance.onGoRightLanePressed += MovePlayerToRightLane;

        _isReady = true;
        playerCurrentLane = 1;

    }

    private void OnDisable()
    {
        InputManager.Instance.onGoLeftLanePressed -= MovePlayerToLeftLane;
        InputManager.Instance.onGoRightLanePressed -= MovePlayerToRightLane;
    }

    void SetupPlayerSpeed()
    {
        float playerSpeed = 1f;
        float distanceBetweenBeats = SoundCreator.Instance.distanceBetweenNotes;
        float secPerBeat = SoundCreator.Instance.secPerBeat;

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
        playerCurrentLane++;
        ChangeDutch(playerCurrentLane);

        playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
        playerController.ChangeLane(GetLanePosition(playerCurrentLane));
        
    }

    public void MovePlayerToLeftLane()
    {
        playerCurrentLane--;
        ChangeDutch(playerCurrentLane);

        playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
        playerController.ChangeLane(GetLanePosition(playerCurrentLane));
    }

    private void ChangeDutch(int lane)
    {
        Debug.Log("switch lane on = "+lane);
        switch(lane)
        {
            case 0 : // Gauche
                cvm.m_Lens.Dutch = -20;
                playerHead.localPosition = new Vector3(1,playerHead.localPosition.y,playerHead.localPosition.z); 
                playerController.animationTrigger.PlayAnimation(AnimationTrigger.AnimationEnum.LeftRun);
            break;

            case 1 : // centre
                cvm.m_Lens.Dutch = 0;
                playerHead.localPosition = new Vector3(0, playerHead.localPosition.y, playerHead.localPosition.z);
                playerController.animationTrigger.PlayAnimation(AnimationTrigger.AnimationEnum.Run);
            break;

            case 2 : // droite
                cvm.m_Lens.Dutch = 20;
                playerHead.localPosition = new Vector3(-1,playerHead.localPosition.y,playerHead.localPosition.z);
                playerController.animationTrigger.PlayAnimation(AnimationTrigger.AnimationEnum.RightRun); 
            break;
        }

    }

    public Vector3 GetLanePosition(int targetLane)
    {
        //Debug.Log("<color=green>go to lane "+targetLane+"</color>");
        //Debug.Log("<color=green>lane position = "+lanes[targetLane].position+"</color>");
        return lanes[targetLane].position;
    }

    public void InitDistanceBop(){
        print("InitBop");
        foreach(GameObject bop in GameObject.FindGameObjectsWithTag("Bop")){
            bop.GetComponentInChildren<SpeedDrone>().InitDistance();
        }

    }
}
