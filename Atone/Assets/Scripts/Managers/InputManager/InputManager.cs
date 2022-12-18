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
    [Header("Switch Action")]
    #region switchlane
    //     SWITCH
    // RIGHT LANE
    public PlayerAction goToRightLane = new PlayerAction("rightLane", KeyCode.Space, KeyCode.A);
    public delegate void OnGoRightLane();
    public OnGoRightLane onGoRightLane;
    // LEFT LANE
    public PlayerAction goToLeftLane = new PlayerAction("leftLane", KeyCode.Space, KeyCode.A);
    public delegate void OnGoLeftLane();
    public OnGoLeftLane onGoLeftLane;

    //    BEND
    // RIGHT LANE
    public PlayerAction bendToRightLane = new PlayerAction("bendRightLane", KeyCode.E, KeyCode.A);
    public delegate void OnBendRightLane(int direction);
    public OnBendRightLane onBendRightLane;
    // LEFT LANE
    public PlayerAction bendToLeftLane = new PlayerAction("bendLeftLane", KeyCode.A, KeyCode.A);
    public delegate void OnBendLeftLane(int direction);
    public OnBendLeftLane onBendLeftLane;
    #endregion

    [Header("Destroy Wall Action")]
    public PlayerAction destroyWallAction = new PlayerAction("destroy", KeyCode.Z, KeyCode.E);
    public delegate void OnDestroyWall();
    public OnDestroyWall onDestroyWall;
    public delegate void OnDestroyWallPressed();
    public OnDestroyWallPressed onDestroyWallPressed;


    void Update()
    {
        // DESTROY WALL
        if (destroyWallAction.GetAction(onController))
            if (onDestroyWall != null)
                onDestroyWall();

        // DESTROY BOP
        if (destroyBopAction.GetAction(onController))
            if (onDestroyBop != null)
                onDestroyBop();

        // SWITCH LANE
        if (goToLeftLane.GetAction(onController) && bendToLeftLane.GetActionPressed(onController))
            if (onGoLeftLane != null)
                onGoLeftLane();

        if (goToRightLane.GetAction(onController) && bendToRightLane.GetActionPressed(onController))
            if (onGoRightLane != null)
                onGoRightLane();

        // BEND ON RIGHT/LEFT LANE


        if (bendToRightLane.GetActionReleased(onController))
        {
            if (!bendToLeftLane.GetActionPressed(onController))
            {
                if (onBendRightLane != null)
                    onBendRightLane(1);
            }
            else
            {
                if (onBendRightLane != null)
                    onBendRightLane(2);
            }
        }

        if (bendToLeftLane.GetActionReleased(onController))
        {
            if (!bendToRightLane.GetActionPressed(onController))
            {
                if (onBendLeftLane != null)
                    onBendLeftLane(1);
            }
             else
            {
                if (onBendRightLane != null)
                    onBendRightLane(0);
            }
        }

        if (bendToRightLane.GetAction(onController))
            if (onBendRightLane != null)
                onBendRightLane(2);

        if (bendToLeftLane.GetAction(onController))
            if (onBendLeftLane != null)
                onBendLeftLane(0);


        // OPEN MENU
        if (menuOrReturn.GetAction(onController))
        {
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

