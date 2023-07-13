using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfIsMoney : MonoBehaviour
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private BuyModule _buyModule;

    private bool _isShowed;

    private void OnEnable()
    {
        _buyModule.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _buyModule.OnMoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        if (_isShowed)
        {
            return;
        }
        if(money == 0)
        {
            return;
        }

        _isShowed = true;

        _visual.SetActive(true);
    }
}
