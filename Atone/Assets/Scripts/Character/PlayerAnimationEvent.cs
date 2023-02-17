using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Cinemachine;

public class PlayerAnimationEvent : MonoBehaviour
{

    public VisualEffect VFXSlide;
    public VisualEffect VFXSnapLeft;
    public VisualEffect VFXSnapRight;
    public VisualEffect VFXLeanLeft;
    public VisualEffect VFXLeanRight;
    public VisualEffect VFXWallrunLeft;
    public VisualEffect VFXWallrunRight;
    public VisualEffect VFXBreakLeft;
    public VisualEffect VFXBreakRight;
    public VisualEffect VFXLaneSwitchLeft;
    public VisualEffect VFXLaneSwitchRight;
    public VisualEffect VFXSpeedLines;
    public PlayerController playerController;

    public void VFXWallrunLeftTrigger()
    {
        VFXWallrunLeft.Play();
        VFXSpeedLines.Play();
        VFXLaneSwitchLeft.Stop();

    }

    public void VFXWallrunRightTrigger()
    {
        VFXWallrunRight.Play();
        VFXSpeedLines.Play();
        VFXLaneSwitchRight.Stop();
    }

    public void VFXWallrunStopTrigger()
    {
        VFXWallrunLeft.Stop();
        VFXWallrunRight.Stop();
        VFXSpeedLines.Play();
    }

    public void VFXSlideTrigger()
    {
        VFXSlide.Play();
    }

    public void VFXSlideStopTrigger()
    {
        VFXSlide.Stop();
    }

    public void VFXSnapLeftTrigger()
    {
        VFXSnapLeft.Play();
    }

    public void VFXSnapRightTrigger()
    {
        VFXSnapRight.Play();
    }

    public void VFXLeanLeftTrigger()
    {

    }

    public void VFXLeanRightTrigger()
    {

    }

    public void VFXBreakLeftTrigger()
    {
        VFXBreakLeft.Play();
    }

    public void VFXBreakRightTrigger()
    {
        VFXBreakRight.Play();
    }

    public void VFXLaneSwitchLeftTrigger()
    {
        VFXSpeedLines.Stop();
        VFXLaneSwitchLeft.Play();
        //Debug.Log("SwitchLeftFVX");
    }

    public void VFXLaneSwitchRightTrigger()
    {
        VFXSpeedLines.Stop();
        VFXLaneSwitchRight.Play();
        //Debug.Log("SwitchRightFVX");
    }

    public void ResetSnapBloomColor()
    {
        PostProcessManager.Instance.ResetBloomColor(0.2f);
    }

    public void ResetBreakBloomColor()
    {
        PostProcessManager.Instance.ResetBloomColor(0.2f);
    }
}
