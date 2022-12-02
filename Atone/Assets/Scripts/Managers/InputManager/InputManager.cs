using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public static bool onController = false;

    [Header("Jump Action")]
    #region jump
    public Action jumpAction = new Action("jump", KeyCode.Space, KeyCode.A);
    public delegate void OnJump();
    public OnJump onJump;
    public delegate void OnJumpPressed();
    public OnJumpPressed onJumpPressed;

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

    [Header("Destroy Action")]
    public Action destroyAction = new Action("destroy", KeyCode.Z, KeyCode.E);
    public delegate void OnDestroy();
    public OnJump onDestroy;
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
            if (onDestroy != null)
                onDestroy();
                
        if (destroyAction.GetActionPressed(onController))
            if (onDestroyPressed != null)
                onDestroyPressed();

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

