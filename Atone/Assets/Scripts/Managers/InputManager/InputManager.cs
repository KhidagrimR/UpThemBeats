using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public static bool onController = false;

    [Header("Break Bop Action")]
    #region BreakBop
    public Action destroyBopAction = new Action("jump", KeyCode.S, KeyCode.S);
    public delegate void OnDestroyBop();
    public OnDestroyBop onDestroyBop;
    public delegate void OnDestroyBopPressed();
    public OnDestroyBopPressed onDestroyBopPressed;

    #endregion

    #region switchlane
    // RIGHT LANE
    public Action goToRightLane = new Action("rightLane", KeyCode.E, KeyCode.A);
    public delegate void OnGoRightLanePressed();
    public OnGoRightLanePressed onGoRightLanePressed;
    // LEFT LANE
    public Action goToLeftLane = new Action("leftLane", KeyCode.A, KeyCode.A);
    public delegate void OnGoLeftLanePressed();
    public OnGoLeftLanePressed onGoLeftLanePressed;
    #endregion

    [Header("Destroy Wall Action")]
    public Action destroyWallAction = new Action("destroy", KeyCode.Z, KeyCode.E);
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

        if (goToLeftLane.GetAction(onController))
            if (onGoLeftLanePressed != null)
                onGoLeftLanePressed();

        if (goToRightLane.GetAction(onController))
            if (onGoRightLanePressed != null)
                onGoRightLanePressed();
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

