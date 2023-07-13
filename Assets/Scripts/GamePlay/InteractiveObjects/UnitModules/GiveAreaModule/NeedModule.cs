using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedModule : UnitModule
{
    public List<CarringObjectType> _needObjectList = new List<CarringObjectType>();

    protected override void Awake()
    {
        _interactiveType = InteractiveType.CanGiveArea;
        base.Awake();
    }
    public override bool IsCanInteract(CarringObject carringObject)
    {
        for(int i = 0; i < _needObjectList.Count; i++)
        {
            if(carringObject.CarringObjectType == _needObjectList[i])
            {
                return true;
            }
        }
        return false;
    }
    public override void ActionInInterract(CarringObjectType carringObjectType)
    {
        _needObjectList.Remove(carringObjectType);
    }
}
