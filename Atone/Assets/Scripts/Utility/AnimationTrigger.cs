using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;

    public enum AnimationEnum {Spawn, Jump, Death, Hit, Idle, Run, Disactivated };

    private AnimationEnum currentAnimation;

    public void PlayAnimation(AnimationEnum triggerName, bool overrideAnimIfIdentical = true)
    {
        if(!overrideAnimIfIdentical)
        {
            if(currentAnimation == triggerName) return;
        }

        currentAnimation = triggerName;
        switch(triggerName)
        {
            case AnimationEnum.Idle:
                animator.SetTrigger("Idle");
            break;

            case AnimationEnum.Spawn:
                animator.SetTrigger("Spawn");
            break;

            case AnimationEnum.Run:
                animator.SetTrigger("Run");
            break;

            case AnimationEnum.Death:
                animator.SetTrigger("Death");
            break;

            case AnimationEnum.Hit:
                animator.SetTrigger("Hit");
            break;

            case AnimationEnum.Jump:
                animator.SetTrigger("Jump");
            break;

            case AnimationEnum.Disactivated:
                animator.SetTrigger("Disactivated");
            break;
        }
        
    }
}
