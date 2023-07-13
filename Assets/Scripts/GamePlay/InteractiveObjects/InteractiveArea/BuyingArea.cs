using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyingArea : InteraciveArea
{
    private const int MONEY_DECREMET_STEP = 9; 

    public event Action OnBuyAction;

    [SerializeField] private TextMeshPro _costTMtext;
    [SerializeField] private int _startCost;
    [SerializeField] private int _costAdderPerLevel;

    private int _leftMoneyInBuyModule;
    private int _currentLevel;
    private int _currentCost;
    private void Awake()
    {
        if(_currentCost == 0)
            _currentCost = _startCost;
        _interactiveType = InteractiveType.Buying;
    }
    private void Start()
    {
        _costTMtext.text = _currentCost.ToString();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    public override void OnTriggeredUnit(List<Unit> unit)
    {
        bool isCanInteract = false;

        for (int i = 0; i < _currentUnit.Count; i++)
        {
            isCanInteract = isCanInteract || (unit[i].InteractiveListType.Contains(_interactiveType) && unit[i].UnitModuleList[unit[i].InteractiveListType.IndexOf(_interactiveType)].IsCanInteract(out _leftMoneyInBuyModule) && !unit[i].IsMove);
        }

        if (isCanInteract)
        {
            base.OnTriggeredUnit(unit);
        }
    }

    protected override void OnValideTriggerAction(Unit unit)
    {
        int bufStep = _leftMoneyInBuyModule > MONEY_DECREMET_STEP ? MONEY_DECREMET_STEP : _leftMoneyInBuyModule;
        bufStep = _currentCost > bufStep ? bufStep : _currentCost;

        unit.UnitModuleList[unit.InteractiveListType.IndexOf(_interactiveType)].ActionInInterract(transform, bufStep);
        _currentCost-= bufStep;
        if(_currentCost <= 0)
        {
            _currentLevel++;
            _currentCost = _startCost + _costAdderPerLevel * _currentLevel;
            OnBuyAction?.Invoke();
        }
        _costTMtext.text = _currentCost.ToString();
    }

    public void BuyAction()
    {
        OnBuyAction?.Invoke();
    }
}
