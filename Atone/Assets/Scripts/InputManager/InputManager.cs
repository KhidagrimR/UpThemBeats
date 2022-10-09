using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public static bool onController = false;

    //jump action
    public static KeyCode jumpActionKeyBoard;
    public static KeyCode jumpActionController;

    public static void setOnController(){
        onController = true;
    }

    public static void setOnKeyboard(){
        onController = false;
    }

    public static KeyCode getActionJump(){
        if(onController)
            return jumpActionController;
        else
            return jumpActionKeyBoard;
    }
}

