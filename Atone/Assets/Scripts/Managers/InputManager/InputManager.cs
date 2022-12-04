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

    [Header("Jump Action")]

    #region jump
    public PlayerAction jumpAction = new PlayerAction("jump", KeyCode.Space, KeyCode.A);

    public delegate void OnJump();
    public OnJump onJump;
    public delegate void OnJumpPressed();
    public OnJumpPressed onJumpPressed;

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

    [Header("Destroy Action")]
    public static PlayerAction destroyAction = new PlayerAction("destroy", KeyCode.W, KeyCode.E);
    public delegate void OnDestroyObstacle();
    public OnJump onDestroyObstacle;
    public delegate void OnDestroyPressed();
    public OnJumpPressed onDestroyPressed;


    void Update()
    {
        if (jumpAction.GetAction(onController))
            if (onJump != null)
                onJump();

        if (jumpAction.GetActionPressed(onController))
            if (onJumpPressed != null)
                onJumpPressed();

        if (destroyAction.GetAction(onController))
            if (onDestroyObstacle != null)
                onDestroyObstacle();
        if (destroyAction.GetActionPressed(onController))
            if (onDestroyPressed != null)
                onDestroyPressed();

        if (goToLeftLane.GetActionPressed(onController))
            if (onGoLeftLanePressed != null)
                onGoLeftLanePressed();

        if (goToRightLane.GetActionPressed(onController))
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

