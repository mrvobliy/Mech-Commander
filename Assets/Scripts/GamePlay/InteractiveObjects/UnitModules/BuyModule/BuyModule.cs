using System;
using UnityEngine;
using TMPro;

public class BuyModule : UnitModule
{
    public event Action<int> OnMoneyChanged;

    [SerializeField] private TextMeshProUGUI _moneyTMPro;
    [SerializeField] private int _money;

    private MoneyPool _moneyPool;
    
    public int Money { get { return _money; } set { _money = value; } }

    protected override void Awake()
    {
        _moneyPool = GetComponentInChildren<MoneyPool>();

        _interactiveType = InteractiveType.Buying;
        base.Awake();
    }

    private void Start()
    {
        ChangeMoney(0);
    }

    public override bool IsCanInteract(out int leftMoney)
    {
        leftMoney = _money;
        return _money > 0;
    }

    public override void ActionInInterract(Transform toObject, int moneyDecrementStep)
    {
        _moneyPool.SpawnMoney(toObject);
        ChangeMoney(-moneyDecrementStep);
    }
    public override void ActionInInterract(int moneyDecrementStep)
    {
        ChangeMoney(moneyDecrementStep);
    }

    public void ChangeMoney(int addValue)
    {
        _money += addValue;
        _moneyTMPro.text = _money.ToString();

        OnMoneyChanged?.Invoke(_money);
    }
}
