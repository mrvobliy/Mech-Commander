using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGiveArea : InteraciveArea
{
    public event Action<CanGiveArea> OnEnabledOnHierarchy;

    [SerializeField] protected CarringObject _objectGive;
    [SerializeField] protected int _currentObjectHasNumber = -1;
    [SerializeField] protected int _maxObjectNumber = -1;

    private List<Vector3> _prevPosition = new List<Vector3>();
    public CarringObject ObjectGive => _objectGive;

    protected virtual void Awake()
    {
        _interactiveType = InteractiveType.CanGiveArea;
        _prevPosition.Add(Vector3.zero);
    }

    protected void OnEnable()
    {
        OnEnabledOnHierarchy?.Invoke(this);
    }

    public override void OnTriggeredUnit(List<Unit> unit)
    {
        if(_currentObjectHasNumber == 0)
        {
            return;
        }

        bool isCanInteract = false;

        for(int i = 0; i < _currentUnit.Count; i++)
        {
            if (i >= _prevPosition.Count)
            {
                _prevPosition.Add(Vector3.zero);
            }

            isCanInteract = isCanInteract || (unit[i].InteractiveListType.Contains(_interactiveType) && unit[i].UnitModuleList[unit[i].InteractiveListType.IndexOf(_interactiveType)].IsCanInteract(_objectGive) && Vector3.Distance(unit[i].transform.position, _prevPosition[i]) < 0.01f);

            _prevPosition[i] = unit[i].transform.position;
        }

        if(isCanInteract)
        {
            base.OnTriggeredUnit(unit);
        }
    }

    protected override void OnValideTriggerAction(Unit unit)
    {
        if (_currentObjectHasNumber == 0 || unit.IsMove || !unit.UnitModuleList[unit.InteractiveListType.IndexOf(_interactiveType)].IsCanInteract(_objectGive))
        {
            return;
        }

        _currentObjectHasNumber--;
        if (_objectGive.IsAbstract)
        {
            unit.UnitModuleList[unit.InteractiveListType.IndexOf(_interactiveType)].ActionInInterract(_objectGive.CarringObjectType);
        }
        else
        {
            CarringObject bufCarringObject = Instantiate(_objectGive, _interactivePoint.position, _interactivePoint.rotation, transform);
            unit.UnitModuleList[unit.InteractiveListType.IndexOf(_interactiveType)].ActionInInterract(bufCarringObject);
        }
    }

    public void AddObjectNumber(int value)
    {
        if(_currentObjectHasNumber >= 0)
        {
            _currentObjectHasNumber += value;
            if(_maxObjectNumber >= 0 && _currentObjectHasNumber > _maxObjectNumber)
            {
                _currentObjectHasNumber = _maxObjectNumber;
            }
        }
    }
    public void SetObjectNumber(int value)
    {
        _currentObjectHasNumber = value;
        if (_maxObjectNumber >= 0 && _currentObjectHasNumber > _maxObjectNumber)
        {
            _currentObjectHasNumber = _maxObjectNumber;
        }
    }
}
