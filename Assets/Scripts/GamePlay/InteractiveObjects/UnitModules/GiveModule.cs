using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveModule : UnitModule
{
    private StackObject _stackObject;

    protected override void Awake()
    {
        _stackObject = GetComponentInChildren<StackObject>();
        _interactiveType = InteractiveType.CanTakeArea;
        base.Awake();
    }

    public override bool IsCanInteract()
    {

        return _stackObject.ObjectStack.Count > 0;
    }
    public override bool IsCanInteract(CarringObjectType carringObjectType)
    {
        bool isContain = false;
        for (int i = 0; i < _stackObject.ObjectStack.Count; i++)
        {
            if(_stackObject.ObjectStack[i].CarringObjectType == carringObjectType)
            {
                isContain = true;
            }
        }
        
        return isContain;
    }

    public override void ActionInInterract(Transform toObject)
    {
        _stackObject.TransferObjectTo(_stackObject.ObjectStack[_stackObject.ObjectStack.Count - 1], toObject);
    }
    public override void ActionInInterract(Transform toObject, CarringObjectType carringObjectType, out int cost)
    {
        cost = 0;
        for (int i = 0; i < _stackObject.ObjectStack.Count; i++)
        {
            if (_stackObject.ObjectStack[i].CarringObjectType == carringObjectType)
            {
                cost = _stackObject.ObjectStack[i].Cost;
                _stackObject.TransferObjectTo(_stackObject.ObjectStack[i], toObject);
                
                return;
            }
        }
        
    }
}
