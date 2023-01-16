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
    public PlayerAction bendToLeftLane = new PlayerAction("bendLeftLane", KeyCode.A, KeyCode.A);
    public delegate void OnBendLane(int direction);
    public OnBendLane onBendLane;

    public delegate void OnBendReleaseLane(int direction);
    public OnBendReleaseLane onBendReleaseLane;
    #endregion

    [Header("Destroy Wall Action")]
    public PlayerAction destroyWallAction = new PlayerAction("destroy", KeyCode.Z, KeyCode.E);
    public delegate void OnDestroyWall();
    public OnDestroyWall onDestroyWall;
    public delegate void OnDestroyWallPressed();
    public OnDestroyWallPressed onDestroyWallPressed;

    // SLIDE
    public PlayerAction slide = new PlayerAction("slide", KeyCode.X, KeyCode.X);
    public delegate void OnSlide(bool slide);
    public OnSlide onSlide;

    // Bend update state vars
    float rightPressedTime = 0; // store the last moment the right bend was pressed
    float leftPressedTime = 0; // store the last moment the left bend was pressed

    void Update()
    {
        #region UI
        // OPEN MENU
        if (menuOrReturn.GetAction(onController))
        {
            // Current setup is hacky, need to change it later. Might need to add a future check to verify that we are not in the main menu scene            
            GameManager.Instance.TogglePauseState();
            onMenu?.Invoke(GameManager.Instance.isGameCurrentlyPaused);
        }
        #endregion
        #region Slide
        if(slide.GetActionReleased(onController))
        {
            if (onSlide != null)
                onSlide(false);
        }
        if (slide.GetAction(onController))
        {
            if (onSlide != null)
                onSlide(true);
            return;
        }
        #endregion
        #region Destroy Obstacle
        // DESTROY WALL
        if (destroyWallAction.GetAction(onController))
            if (onDestroyWall != null)
                onDestroyWall();

        // DESTROY BOP
        if (destroyBopAction.GetAction(onController))
            if (onDestroyBop != null)
                onDestroyBop();

        #endregion
        #region switch lane
        // SWITCH LANE
        if (goToLeftLane.GetAction(onController) && bendToLeftLane.GetActionPressed(onController))
            if (onGoLeftLane != null)
                onGoLeftLane();

        if (goToRightLane.GetAction(onController) && bendToRightLane.GetActionPressed(onController))
            if (onGoRightLane != null)
                onGoRightLane();
        #endregion
        #region Bend

        #region commented
        // BEND ON RIGHT/LEFT LANE
        /*if (bendToRightLane.GetActionReleased(onController))
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
        }*/
        /*
        // Quand le joueur appuye sur "Droite"
        if ()
        {
            // si il est à gauche
            if (bendToLeftLane.GetActionPressed(onController))
            {
                // il va à droite
                if (onBendRightLane != null)
                    onBendRightLane(2);
            }
            // si il relache le bouton gauche
            if (bendToLeftLane.GetActionReleased(onController))
            {
                // il va à droite
                if (onBendRightLane != null)
                    onBendRightLane(2);
            }

            // si il appuye sur gauche


            // il va à droite
            if (onBendRightLane != null)
                onBendRightLane(2);
        }
        // Quand le joueur appuye sur "Gauche"
        else if (bendToLeftLane.GetAction(onController))
        {
            // il va a gauche
            if (onBendLeftLane != null)
                onBendLeftLane(0);
        }
*/

        #endregion

        bool rightPressed = bendToRightLane.GetAction(onController);
        bool rightMaintained = bendToRightLane.GetActionPressed(onController);
        bool rightReleased = bendToRightLane.GetActionReleased(onController);

        bool leftPressed = bendToLeftLane.GetAction(onController);
        bool leftMaintained = bendToLeftLane.GetActionPressed(onController);
        bool leftReleased = bendToLeftLane.GetActionReleased(onController);

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


    public static void SetOnController()
    {
        onController = true;
    }

    public static void SetOnKeyboard()
    {
        onController = false;
    }
}

