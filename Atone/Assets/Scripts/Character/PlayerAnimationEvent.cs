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
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Standing);
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
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Snap);
    }

    public void VFXSnapRightTrigger()
    {
        VFXSnapRight.Play();
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Snap);
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
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Break);
    }

    public void VFXBreakRightTrigger()
    {
        VFXBreakRight.Play();
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Break);
    }

    public void VFXLaneSwitchLeftTrigger()
    {
        VFXSpeedLines.Stop();
        VFXLaneSwitchLeft.Play();
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.WallrunLeft);
        //Debug.Log("SwitchLeftFVX");
    }

    public void VFXLaneSwitchRightTrigger()
    {
        VFXSpeedLines.Stop();
        VFXLaneSwitchRight.Play();
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.WallrunRight);
        //Debug.Log("SwitchRightFVX");
    }

    public void ResetSnapBloomColor()
    {
        PostProcessManager.Instance.ResetBloomColor(0.2f);
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Standing);
    }

    public void ResetBreakBloomColor()
    {
        PostProcessManager.Instance.ResetBloomColor(0.2f);
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Standing);
    }

    public void HitRecovered()
    {
        PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.Standing);
    }
}
