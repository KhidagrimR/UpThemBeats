using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Action
{
    [SerializeField]
    private string name;
    [SerializeField]
    private KeyCode actionKeyBoard;
    [SerializeField]
    private KeyCode actionController;

    public Action(string name, KeyCode actionKeyBoard, KeyCode actionController){
        this.name = name;
        this.actionKeyBoard = actionKeyBoard;
        this.actionController = actionController;
    }

    public bool GetAction(bool onController){
        if(onController)
            return Input.GetKeyDown(actionController);
        else
            return Input.GetKeyDown(actionKeyBoard);
    }

    public bool GetActionPressed(bool onController){
        if(onController)
            return Input.GetKey(actionController);
        else
            return Input.GetKey(actionKeyBoard);
    }
}
