using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimatorParams
{    
    private static Dictionary<AnimationEnum,int> animParams = new Dictionary<AnimationEnum, int>();
    // Order: Spawn, Jump, Death, Hit, Idle, Run, Disactivated, LeftRun, RightRun, LeftSnapPrepare
    public static Dictionary<AnimationEnum,int> PlayerAnimParamsDict {get => animParams;}

    public static void BuildDictionary()
    {
        animParams.Add(AnimationEnum.Spawn, Animator.StringToHash("Spawn"));
        animParams.Add(AnimationEnum.Death, Animator.StringToHash("Death"));
        animParams.Add(AnimationEnum.HitTaken, Animator.StringToHash("Hit"));
        animParams.Add(AnimationEnum.Idle, Animator.StringToHash("Idle"));
        animParams.Add(AnimationEnum.Disactivated, Animator.StringToHash("Disactivated"));
        animParams.Add(AnimationEnum.LeftSnapPrepare, Animator.StringToHash("LeftSnapPrepare"));
    }
}
