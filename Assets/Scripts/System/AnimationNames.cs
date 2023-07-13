using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationNamesEnum
{
    Walk, Run, Carry, Sit, Lay, Stand, Idle, PushButton
}
public class AnimationNames
{
    private string[] ProperetyName = { "Walk", "Run", "Carry", "Sit", "Lay", "Stand", "Idle", "PushButton" };
    public static string GetAnimationStringName(AnimationNamesEnum enumName)
    {
        AnimationNames animationNames = new AnimationNames();
        return animationNames.ProperetyName[(int)enumName];
    }
}
