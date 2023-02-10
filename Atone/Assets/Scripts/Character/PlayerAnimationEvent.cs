using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

    public PlayerController playerController;

    public void VFXWallrunLeftTrigger()
    {
        VFXWallrunLeft.Play();
    }

    public void VFXSlideTrigger()
    {
        VFXSlide.Play();
    }

    public void VFXWallrunRightTrigger()
    {
        VFXWallrunRight.Play();
    }

    public void VFXSlideStopTrigger()
    {
        VFXSlide.Stop();
    }

    public void VFXWallrunStopTrigger()
    {
        VFXWallrunLeft.Stop();
        VFXWallrunRight.Stop();
    }
}
