using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingObject : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectOnBuy;
    [SerializeField] private int _currentIndexBuyedObject = -1;
    [SerializeField] private int _experienceValue = 2;

    private BuyingArea _buyingArea;
    public int CurrentIndexBuyedObject => _currentIndexBuyedObject;

    private void Awake()
    {
        _buyingArea = GetComponentInChildren<BuyingArea>(true);
        _buyingArea.OnBuyAction += OnBuyAction;
    }

    private void OnDestroy()
    {
        _buyingArea.OnBuyAction -= OnBuyAction;
    }

    private void OnBuyAction()
    {
        GameEvents.instance.BuyObject(_experienceValue);

        _currentIndexBuyedObject++;
        _buyingArea.gameObject.SetActive(false);
        if (_currentIndexBuyedObject >= _objectOnBuy.Length)
        {
            // max level upgrade
            return;
        }

        _objectOnBuy[_currentIndexBuyedObject].SetActive(true);
        if (_currentIndexBuyedObject - 1 < 0)
        {
            return;
        }
        _objectOnBuy[_currentIndexBuyedObject - 1].SetActive(false);
    }
}
