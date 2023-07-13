using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarringModule : UnitModule
{
    private StackObject _stackCarry;

    protected override void Awake()
    {
        _interactiveType = InteractiveType.CanGiveArea;
        _stackCarry = GetComponentInChildren<StackObject>();
        base.Awake();
    }

    public override bool IsCanInteract( CarringObject carringObject)
    {
        return _stackCarry.IsHasEmptySlot();
    }

    public override void ActionInInterract(CarringObject carringObject)
    {
        _stackCarry.AddObject(carringObject);
    }
}
