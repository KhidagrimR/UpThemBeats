using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public static bool onController = false;

    [Header("Jump Action")]
    public static Action jumpAction = new Action("jump",KeyCode.Space,KeyCode.A);

    public delegate void OnJump();
    public OnJump onJump;
    public delegate void OnJumpPressed();
    public OnJumpPressed onJumpPressed;

    [Header("Destroy Action")]

    public static Action destroyAction = new Action("destroy",KeyCode.W,KeyCode.E);

    public delegate void OnDestroy();
    public OnJump onDestroy;
    public delegate void OnDestroyPressed();
    public OnJumpPressed onDestroyPressed;


    void Update()
    {
        if(jumpAction.GetAction(onController))
            if(onJump != null)
                onJump();

        if (jumpAction.GetActionPressed(onController))
            if(onJumpPressed != null)
                onJumpPressed();
        
        if(destroyAction.GetAction(onController))
            if(onDestroy != null)
                onDestroy();
        if(destroyAction.GetActionPressed(onController))
            if(onDestroyPressed != null)
                onDestroyPressed();
    }


    public static void SetOnController(){
        onController = true;
    }

    public static void SetOnKeyboard(){
        onController = false;
    }
}

