using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GetModule : UnitModule
{
    public List<CarringObjectType> _currentNeedObjects;
    protected override void Awake()
    {
        _interactiveType = InteractiveType.CanGiveArea;
        base.Awake();
    }

    public override bool IsCanInteract(CarringObject carringObject)
    {
        return _currentNeedObjects.Contains(carringObject.CarringObjectType);
    }

    public override void ActionInInterract(CarringObject carringObject)
    {
        _currentNeedObjects.Remove(carringObject.CarringObjectType);
        carringObject.transform.SetParent(transform);
        carringObject.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.35f).SetAutoKill(true).OnKill(() => Destroy(carringObject.gameObject));
    }
}
