using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Action
{
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private KeyCode m_ActionKeyBoard;
    [SerializeField]
    private KeyCode m_ActionController;

    public Action(string name, KeyCode actionKeyBoard, KeyCode actionController){
        this.m_Name = name;
        this.m_ActionKeyBoard = actionKeyBoard;
        this.m_ActionController = actionController;
    }

    public bool GetAction(bool onController){
        if(onController)
            return Input.GetKeyDown(m_ActionController);
        else
            return Input.GetKeyDown(m_ActionKeyBoard);
    }

    public bool GetActionPressed(bool onController){
        if(onController)
            return Input.GetKey(m_ActionController);
        else
            return Input.GetKey(m_ActionKeyBoard);
    }
}
