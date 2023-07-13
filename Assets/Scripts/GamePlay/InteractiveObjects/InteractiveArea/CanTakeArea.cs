using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTakeArea : InteraciveArea
{
    private List<Vector3> _prevPosition = new List<Vector3>();
    private void Awake()
    {
        _interactiveType = InteractiveType.CanTakeArea;
        _prevPosition.Add(Vector3.zero);
    }

    public override void OnTriggeredUnit(List<Unit> unit)
    {
        bool isCanInteract = false;

        for (int i = 0; i < _currentUnit.Count; i++)
        {
            if (i >= _prevPosition.Count)
            {
                _prevPosition.Add(Vector3.zero);
            }

            isCanInteract = isCanInteract || (unit[i].InteractiveListType.Contains(_interactiveType) && unit[i].UnitModuleList[unit[i].InteractiveListType.IndexOf(_interactiveType)].IsCanInteract() && unit[i].transform.position == _prevPosition[i]);

            _prevPosition[i] = unit[i].transform.position;
        }

        if (isCanInteract)
        {
            base.OnTriggeredUnit(unit);
        }
    }

    protected override void OnValideTriggerAction(Unit unit)
    {
        unit.UnitModuleList[unit.InteractiveListType.IndexOf(_interactiveType)].ActionInInterract(_interactivePoint);
    }
}
