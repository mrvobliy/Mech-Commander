using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanModule : UnitModule
{
    protected override void Awake()
    {
        _interactiveType = InteractiveType.Cleaning;
        base.Awake();
    }

    public override bool IsCanInteract()
    {
        return true;
    }
}
