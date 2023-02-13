using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerAction
{
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private KeyCode m_ActionKeyBoardRightHanded;
    [SerializeField]
    private KeyCode m_ActionKeyBoardLeftHanded;
    [SerializeField]
    private KeyCode m_ActionController;

    public enum Axis {
        None,
        StickleftUp,
        StickleftDown,
        StickleftRight,
        StickleftLeft,
        SharedTriggerRTLT,
        RT,
        LT
    }
    [SerializeField]
    private Axis m_axis;

    public PlayerAction(string name, KeyCode actionKeyBoardRightHanded, KeyCode actionKeyBoardLeftHanded, KeyCode actionController, Axis axis){
        this.m_Name = name;
        this.m_ActionKeyBoardRightHanded = actionKeyBoardRightHanded;
        this.m_ActionKeyBoardLeftHanded = actionKeyBoardLeftHanded;
        this.m_ActionController = actionController;
        this.m_axis = axis;
    }

    public bool GetActionDown(bool onController, bool isRightHanded){
        if (onController){
            if (m_axis == Axis.None)
                return Input.GetKeyDown(m_ActionController);
            else
                return GetAxisDirectionTrigger();
        }

        else
            if(isRightHanded)
                return Input.GetKeyDown(m_ActionKeyBoardRightHanded);
            else
                return Input.GetKeyDown(m_ActionKeyBoardLeftHanded);
    }

    public bool GetActionPressed(bool onController, bool isRightHanded){
        if(onController){
            if (m_axis == Axis.None)
                return Input.GetKey(m_ActionController);
            else
                return GetAxisDirectionTrigger();
        }
        else
            if(isRightHanded)
                return Input.GetKey(m_ActionKeyBoardRightHanded);
            else 
                return Input.GetKey(m_ActionKeyBoardLeftHanded);
    }

    public bool GetActionReleased(bool onController, bool isRightHanded)
    {
        if(onController){
            if (m_axis == Axis.None)
                return Input.GetKeyUp(m_ActionController);
            else
                return GetAxisDirectionTrigger();
        }
        else
            if(isRightHanded)
                return Input.GetKeyUp(m_ActionKeyBoardRightHanded);
            else
                return Input.GetKeyUp(m_ActionKeyBoardLeftHanded);
    }

    public bool GetAxisDirectionTrigger() {
        switch (m_axis){
            case Axis.StickleftUp:
                return Input.GetAxis("StickleftVertical") > 0;
            case Axis.StickleftDown:
                return Input.GetAxis("StickleftVertical") < 0;
            case Axis.StickleftRight:
                return Input.GetAxis("StickleftHorizontal") > 0;
            case Axis.StickleftLeft:
                return Input.GetAxis("StickleftHorizontal") < 0;
            case Axis.SharedTriggerRTLT:
                return Input.GetAxis("SharedTriggerRTLT") > 0;
            case Axis.RT:
                return Input.GetAxis("RT") > 0;
            case Axis.LT:
                return Input.GetAxis("LT") > 0;
            default:
                return false;
        }
    }
}
