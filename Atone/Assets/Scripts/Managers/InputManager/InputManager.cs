using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager : Singleton<InputManager>
{
    public static bool onController = false;

    public static bool isRightHanded = false;
    [Header("Pause menu / return")]

    public PlayerAction menuOrReturn;

    public delegate void OnMenu(bool isPaused);
    public static event OnMenu onMenu; // Needs to be static to not mess things up when communicating to the additive scene


    [Header("Break Bop Action")]
    #region BreakBop
    public PlayerAction destroyBopAction;
    public delegate void OnDestroyBop();
    public OnDestroyBop onDestroyBop;
    public delegate void OnDestroyBopPressed();
    public OnDestroyBopPressed onDestroyBopPressed;

    #endregion
    [Header("Switch Action")]
    #region switchlane
    //     SWITCH
    // RIGHT LANE
    public PlayerAction goToRightLane;
    public delegate void OnGoRightLane();
    public OnGoRightLane onGoRightLane;
    // LEFT LANE
    public PlayerAction goToLeftLane;
    public delegate void OnGoLeftLane();
    public OnGoLeftLane onGoLeftLane;

    //    BEND
    // RIGHT LANE
    public PlayerAction bendToRightLane;
    public PlayerAction bendToLeftLane;
    public delegate void OnBendLane(int direction);
    public OnBendLane onBendLane;

    public delegate void OnBendReleaseLane(int direction);
    public OnBendReleaseLane onBendReleaseLane;
    #endregion

    [Header("Destroy Wall Action")]
    public PlayerAction destroyWallAction;
    public delegate void OnDestroyWall();
    public OnDestroyWall onDestroyWall;
    public delegate void OnDestroyWallPressed();
    public OnDestroyWallPressed onDestroyWallPressed;

    // SLIDE
    public PlayerAction slide;
    public delegate void OnSlide(bool slide);
    public OnSlide onSlide;

    // Bend update state vars
    float rightPressedTime = 0; // store the last moment the right bend was pressed
    float leftPressedTime = 0; // store the last moment the left bend was pressed

    void Update()
    {
        if(!GameManager.Instance.isReady) return;
        #region UI
        // OPEN MENU
        if (menuOrReturn.GetAction(onController,isRightHanded))
        {
            // Current setup is hacky, need to change it later. Might need to add a future check to verify that we are not in the main menu scene            
            GameManager.Instance.TogglePauseState();
            onMenu?.Invoke(GameManager.Instance.isGameCurrentlyPaused);
        }
        #endregion
        #region Slide
        if(slide.GetActionReleased(onController, isRightHanded))
        {
            if (onSlide != null)
                onSlide(false);
        }
        if (slide.GetAction(onController, isRightHanded))
        {
            if (onSlide != null)
                onSlide(true);
            return;
        }
        #endregion
        CheckIfControllerOrKeyBoardIsActive();
        //print("onController" + onController);
        #region Destroy Obstacle
        // DESTROY WALL
        if (destroyWallAction.GetAction(onController, isRightHanded))
            if (onDestroyWall != null)
                onDestroyWall();

        // DESTROY BOP
        if (destroyBopAction.GetAction(onController, isRightHanded))
            if (onDestroyBop != null)
            {
                onDestroyBop();
                Debug.Log("player pos = " + PlayerManager.Instance.playerController.transform.position);

            }

        #endregion
        #region switch lane
        // SWITCH LANE
        if (goToLeftLane.GetAction(onController,isRightHanded) && bendToLeftLane.GetActionPressed(onController,isRightHanded))
            if (onGoLeftLane != null)
                onGoLeftLane();

        if (goToRightLane.GetAction(onController,isRightHanded) && bendToRightLane.GetActionPressed(onController,isRightHanded))
            if (onGoRightLane != null)
                onGoRightLane();
        #endregion
        #region Bend

        bool rightPressed = bendToRightLane.GetAction(onController,isRightHanded);
        bool rightMaintained = bendToRightLane.GetActionPressed(onController,isRightHanded);
        bool rightReleased = bendToRightLane.GetActionReleased(onController,isRightHanded);

        bool leftPressed = bendToLeftLane.GetAction(onController,isRightHanded);
        bool leftMaintained = bendToLeftLane.GetActionPressed(onController,isRightHanded);
        bool leftReleased = bendToLeftLane.GetActionReleased(onController,isRightHanded);

        if (rightPressed) rightPressedTime = Time.time;
        if (leftPressed) leftPressedTime = Time.time;

        // Right first and Left first are the same here, if rightfirst = true, then left first is false
        // Use both of them to have a better understanding of your doing
        bool rightMoreRecent = rightPressedTime > leftPressedTime;
        bool leftMoreRecent = leftPressedTime > rightPressedTime;

        /*Debug.Log("###");
        Debug.Log("Right pressed = " + rightPressed + "; right maintained = " + rightMaintained + "; right released = " + rightReleased + ";");
        Debug.Log("left pressed = " + leftPressed + "; left maintained = " + leftMaintained + "; left released = " + leftReleased + ";");
        Debug.Log("###");*/

        int laneToBend = 1;

        if (leftMoreRecent)
        {
            if (leftMaintained || leftPressed)
            {
                laneToBend = 0;
            }
            else if (rightMaintained)
            {
                if (leftPressed)
                {
                    laneToBend = 1;
                }
                else if (leftMaintained)
                {
                    laneToBend = 0;
                }
                else
                    laneToBend = 2;
            }
            else
            {
                laneToBend = 1;
            }
        }
        if (rightMoreRecent)
        {
            if (rightMaintained || rightPressed)
            {
                laneToBend = 2;
            }
            else if (leftMaintained)
            {
                if (rightPressed)
                {
                    laneToBend = 1;
                }
                else if (rightMaintained)
                {
                    laneToBend = 2;
                }
                else
                    laneToBend = 0;
            }
            else
            {
                laneToBend = 1;
            }
        }

        if (onBendLane != null)
            onBendLane(laneToBend);

        if (leftReleased)
        {
            if (onBendReleaseLane != null)
                onBendReleaseLane(0);
        }
        if (rightReleased)
        {
            if (onBendReleaseLane != null)
                onBendReleaseLane(2);
        }

        #endregion

    }

    public void CheckIfControllerOrKeyBoardIsActive() {
        CheckIfAxisIsTrigger();
        foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kc))
            {
                //print("kc" + kc.ToString());
                if (kc.ToString().Contains("Joystick"))
                {

                    onController = true;
                }
                else
                    onController = false;
            }
        }
    }

    public void CheckIfAxisIsTrigger() {
        if (Input.GetAxis("StickleftHorizontal") != 0 || Input.GetAxis("StickleftVertical") != 0 || Input.GetAxis("ShareTriggerRTLT") != 0 ||
            Input.GetAxis("RT") != 0 || Input.GetAxis("LT") != 0 || Input.GetAxis("StickrightHorizontal") != 0 || 
            Input.GetAxis("StickrightHorizontal") != 0 || Input.GetAxis("DirectionalCrossHorizontal") != 0 || Input.GetAxis("DirectionalCrossVertical") != 0)
            onController = true;
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

