using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorModule : UnitModule
{
    protected override void Awake()
    {
        _interactiveType = InteractiveType.CanActivateArea;
        base.Awake();
    }

    public override bool IsCanInteract()
    {
        return true;
    }

    public override void ActionInInterract(CarringObject carringObject)
    {
        
    }
}
