using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeNeedArea : InteraciveArea
{
    public event Action<int> OnGetNeed;

    [SerializeField] private List<CarringObjectType> _needObjects = new List<CarringObjectType>();
    public List<CarringObjectType> NeedObjects => _needObjects;
    private void Awake()
    {
        _interactiveType = InteractiveType.CanTakeArea;
    }
    protected override void OnDisable()
    {
        GameEvents.instance.DisableNeed(this);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Unit>(out Unit bufCurrentUnit) && bufCurrentUnit.InteractiveListType.Contains(_interactiveType))
        {
            for(int i = 0; i < _needObjects.Count; i++)
            {
                if (bufCurrentUnit.UnitModuleList[bufCurrentUnit.InteractiveListType.IndexOf(_interactiveType)].IsCanInteract(_needObjects[i]))
                {
                    DoSomethingAction();
                    bufCurrentUnit.UnitModuleList[bufCurrentUnit.InteractiveListType.IndexOf(_interactiveType)].ActionInInterract(_interactivePoint, _needObjects[i], out int cost);
                    OnGetNeed?.Invoke(cost);
                    _needObjects.RemoveAt(_needObjects.IndexOf(_needObjects[i]));
                }
            }
        }
    }

    public override void OnTriggeredUnit(List<Unit> unit)
    {

    }

    protected override void OnValideTriggerAction(Unit unit)
    {
        
    }

    public void AddNeed(CarringObjectType needType)
    {
        _needObjects.Add(needType);
    }
    public void DisableNeed(CarringObjectType needType)
    {
        GameEvents.instance.DisableNeed(this);
    }
}
