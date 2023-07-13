using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanArea : InteraciveArea
{
    [SerializeField] private Image _filler;
    private void Awake()
    {
        _interactiveType = InteractiveType.Cleaning;
    }
    private void OnEnable()
    {
        GameEvents.instance.AddedToClean(this);
    }
    public override void OnTriggeredUnit(List<Unit> unit)
    {
        base.OnTriggeredUnit(unit);

        _filler.fillAmount = _currentTimeToCallEvent / _timeToCallEvent;
    }
}
