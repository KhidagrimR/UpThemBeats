using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Here is located the dictionary containing all the hashed animator parameters
/// </summary>
public static class GameAnimatorsParams
{    
    private static Dictionary<AnimationEnum,int> animParams;
    // Order: Spawn, Jump, Death, Hit, Idle, Run, Disactivated, LeftRun, RightRun, LeftSnapPrepare
    public static Dictionary<AnimationEnum,int> AnimParamsDict {get => animParams;}

    public static void BuildDictionary()
    {
        Array retrievedValues = Enum.GetValues(typeof(AnimationEnum));
        int numberOfValues = retrievedValues.Length;
        
        animParams = new Dictionary<AnimationEnum, int>
            (numberOfValues)
        ;

        // animParams.Add(AnimationEnum.Spawn, Animator.StringToHash("Spawn"));
        // animParams.Add(AnimationEnum.Jump, Animator.StringToHash("Jump"));
        // animParams.Add(AnimationEnum.Death, Animator.StringToHash("Death"));
        // animParams.Add(AnimationEnum.Hit, Animator.StringToHash("Hit"));
        // animParams.Add(AnimationEnum.Idle, Animator.StringToHash("Idle"));
        // animParams.Add(AnimationEnum.Run, Animator.StringToHash("Run"));
        // animParams.Add(AnimationEnum.Disactivated, Animator.StringToHash("Disactivated"));
        // animParams.Add(AnimationEnum.LeftRun, Animator.StringToHash("LeftRun"));
        // animParams.Add(AnimationEnum.RightRun, Animator.StringToHash("RightRun"));
        // animParams.Add(AnimationEnum.LeftSnapPrepare, Animator.StringToHash("LeftSnapPrepare"));

        // Note importante: si l'on se dit que le nom de l'entrée correspond EXACTEMENT au nom du paramtètre, 
        // alors on peut encore plus simplifier la fonction en appelant pour chaque élément de l'Enum :

        foreach (AnimationEnum stateParam in retrievedValues)
        {
            animParams.Add(stateParam, Animator.StringToHash(stateParam.ToString()));
        }

        // Comme ça, on n'aurait besoin que de modifier l'Enum, sans toucher à BuildDictionary. On évitera aussi un mur de Add() multicolore
    }
}

public enum AnimationEnum 
{
    Spawn, 
    Jump, 
    JumpStop,
    Death, 
    Hit, 
    Idle, 
    Run, 
    Disactivated, 
    LeftRun, 
    RightRun, 
    LeftSnapPrepare,
    SnapLeft,
    SnapRight,
    LeanLeft,
    LeanRight,
    LeanStop,
    SlideStart,
    SlideStop,
    BreakLeft,
    BreakRight,
    SpeedScaleFactor,
    SkateCycleOffset,
};