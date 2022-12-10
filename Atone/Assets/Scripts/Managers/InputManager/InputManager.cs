using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public static bool onController = false;
    [Header("Pause menu / return")]

    public PlayerAction menuOrReturn = new PlayerAction("menuOrReturn", KeyCode.Escape, KeyCode.Joystick1Button7);

    public delegate void OnMenu(bool isPaused);
    public static event OnMenu onMenu; // Needs to be static to not mess things up when communicating to the additive scene


    [Header("Break Bop Action")]
    #region BreakBop
    public PlayerAction destroyBopAction = new PlayerAction("jump", KeyCode.S, KeyCode.S);
    public delegate void OnDestroyBop();
    public OnDestroyBop onDestroyBop;
    public delegate void OnDestroyBopPressed();
    public OnDestroyBopPressed onDestroyBopPressed;

    #endregion

    #region switchlane
    // RIGHT LANE
    public PlayerAction goToRightLane = new PlayerAction("rightLane", KeyCode.E, KeyCode.A);
    public delegate void OnGoRightLanePressed();
    public OnGoRightLanePressed onGoRightLanePressed;
    // LEFT LANE
    public PlayerAction goToLeftLane = new PlayerAction("leftLane", KeyCode.A, KeyCode.A);
    public delegate void OnGoLeftLanePressed();
    public OnGoLeftLanePressed onGoLeftLanePressed;
    #endregion

    [Header("Destroy Wall Action")]
    public PlayerAction destroyWallAction = new PlayerAction("destroy", KeyCode.Z, KeyCode.E);
    public delegate void OnDestroyWall();
    public OnDestroyWall onDestroyWall;
    public delegate void OnDestroyWallPressed();
    public OnDestroyWallPressed onDestroyWallPressed;


    void Update()
    {
        if (destroyWallAction.GetAction(onController))
            if (onDestroyWall != null)
                onDestroyWall();

        if (destroyWallAction.GetActionPressed(onController))
            if (onDestroyWallPressed != null)
                onDestroyWallPressed();

        if (destroyBopAction.GetAction(onController))
            if (onDestroyBop != null)
                onDestroyBop();
                
        if (destroyBopAction.GetActionPressed(onController))
            if (onDestroyBopPressed != null)
                onDestroyBopPressed();

        if (goToLeftLane.GetActionPressed(onController))
            if (onGoLeftLanePressed != null)
                onGoLeftLanePressed();

        if (goToRightLane.GetAction(onController))
            if (onGoRightLanePressed != null)
                onGoRightLanePressed();

        if(menuOrReturn.GetAction(onController)){
            // Current setup is hacky, need to change it later. Might need to add a future check to verify that we are not in the main menu scene            
            GameManager.Instance.TogglePauseState();
            onMenu?.Invoke(GameManager.Instance.isGameCurrentlyPaused);
        }
    }


    public static void SetOnController()
    {
        onController = true;
    }

    public static void SetOnKeyboard()
    {
        onController = false;
    }
}

