using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;

    public VisualEffect vfxArrival;

    public VisualEffect vfxDeath;

    public StudioEventEmitter soundImpact;

    // public enum AnimationEnum {Spawn, Jump, Death, Hit, Idle, Run, Disactivated, LeftRun, RightRun, LeftSnapPrepare };
    // Déplacé hors de la classe (voir plus bas) pour un accès plus facile

    private AnimationEnum currentAnimation;

    public void PlayAnimation(AnimationEnum triggerName, bool overrideAnimIfIdentical = true)
    {
        if (!overrideAnimIfIdentical)
        {
            if (currentAnimation == triggerName) return;
        }

        currentAnimation = triggerName;

        animator.SetTrigger(GameAnimatorsParams.AnimParamsDict[triggerName]);
        PlayArrivalVFX();
    }

    public void PlayArrivalVFX()
    {
        if (vfxArrival != null)
            vfxArrival.Play();
    }

    public void PlayDeathVFX()
    {
        if(vfxDeath != null)
        vfxDeath.Play();
    }


    // coming from griffe wallrun 1|Armature action 003
    public void PlayVFX()
    {

    }

    public void DropCameraShake()
    {
        CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.RedwallDrop);
    }
    
    public void PlaySFX()
    {
        soundImpact.Play();
    }
}
