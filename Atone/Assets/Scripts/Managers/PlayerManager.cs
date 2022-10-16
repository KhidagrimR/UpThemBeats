using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerController playerController;
    
    private bool _isReady;
    public bool isReady
    {
        get{return _isReady;}
    }

    public void Init()
    {
        SetupPlayerSpeed();

        _isReady = true;
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
    }

    void SetupPlayerAnimationSpeed()
    {
        // just a reminder to modify the player animation speed to make it goes up and down on the beats

        // delete the function if that's already done as you read this
    }

}
