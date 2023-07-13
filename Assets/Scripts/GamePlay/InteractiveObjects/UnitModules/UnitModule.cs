using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModule : MonoBehaviour
{
    protected InteractiveType _interactiveType;

    protected virtual void Awake()
    {
        Unit unit = GetComponent<Unit>();
        unit.InteractiveListType.Add(_interactiveType);
        unit.UnitModuleList.Add(this);
    }

    public virtual bool IsCanInteract()
    {
        return false;
    }
    public virtual bool IsCanInteract(out int leftMoney)
    {
        leftMoney = 0;
        return false;
    }
    public virtual bool IsCanInteract(CarringObject carringObject)
    {
        return false;
    }
    public virtual bool IsCanInteract(CarringObjectType carringObjectType)
    {
        return false;
    }

    public virtual void ActionInInterract()
    {

    }
    public virtual void ActionInInterract(int getMoney)
    {

    }
    public virtual void ActionInInterract(Transform toObject, int leftMoney)
    {
        
    }
    public virtual void ActionInInterract(Transform toObject)
    {

    }
    public virtual void ActionInInterract(CarringObject carringObject)
    {

    }
    public virtual void ActionInInterract(CarringObjectType carringObjectType)
    {

    }
    public virtual void ActionInInterract(Transform toObject, CarringObjectType carringObject, out int cost)
    {
        cost = 0;
    }
}
