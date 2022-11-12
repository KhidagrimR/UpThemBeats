using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerController playerController;
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

        InputManager.Instance.onGoLeftLanePressed += MovePlayerToLeftLane;
        InputManager.Instance.onGoRightLanePressed += MovePlayerToRightLane;

        _isReady = true;
        playerCurrentLane = 1;
    }

    private void OnDisable() {
        InputManager.Instance.onGoLeftLanePressed -= MovePlayerToLeftLane;
        InputManager.Instance.onGoRightLanePressed -= MovePlayerToRightLane;
    }

    void SetupPlayerSpeed()
    {
        float playerSpeed;
        float distanceBetweenBeats = SoundCreator.Instance.distanceBetweenNotes;
        float secPerBeat = SoundCreator.Instance.secPerBeat;

        // here you calculate the speed to reach the next "beat" point
        // v = d / t => d = distanceBetweenBeats and t => secPerBeat
        playerSpeed = distanceBetweenBeats / secPerBeat;

        playerController.playerSpeed = playerSpeed;
    }

    void SetupPlayerAnimationSpeed()
    {
        // just a reminder to modify the player animation speed to make it goes up and down on the beats

        // delete the function if that's already done as you read this
    }

    public void MovePlayerToRightLane()
    {
        playerCurrentLane ++;
        playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
        playerController.ChangeLane(GetLanePosition(playerCurrentLane));
    }

    public void MovePlayerToLeftLane()
    {
        playerCurrentLane --;
        playerCurrentLane = Mathf.Clamp(playerCurrentLane, 0, lanes.Length - 1);
        playerController.ChangeLane(GetLanePosition(playerCurrentLane));
    }

    public Vector3 GetLanePosition(int targetLane)
    {
        //Debug.Log("<color=green>go to lane "+targetLane+"</color>");
        //Debug.Log("<color=green>lane position = "+lanes[targetLane].position+"</color>");
        return lanes[targetLane].position;
    }



 /* ### TO TEST PLAYER SPEED ###
    private float testCounter = 1;
    private float currentTestCounter = 1;

    private float playerZ = 0;

    private void Update() {

        if(currentTestCounter <= 0)
        {
            // do the thing
            Debug.Log("Player distance in 1s = "+(playerZ - playerController.transform.position.z));

            playerZ = playerController.transform.position.z;

            currentTestCounter = testCounter;
        }
        else
        {
            currentTestCounter -= Time.deltaTime;
        }
        
    }

    */
}
