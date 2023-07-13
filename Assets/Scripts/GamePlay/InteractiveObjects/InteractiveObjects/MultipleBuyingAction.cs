using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleBuyingAction : MonoBehaviour
{
    [SerializeField] private BuyingArea[] _areasToActivate;
    [SerializeField] private BuyingArea _activatingbuyingArea;

    private void Start()
    {
        _activatingbuyingArea.OnBuyAction += OnBuyAction;
    }

    private void OnDestroy()
    {
        _activatingbuyingArea.OnBuyAction -= OnBuyAction;
    }

    private void OnBuyAction()
    {
        for(int i = 0; i < _areasToActivate.Length; i++)
        {
            _areasToActivate[i].BuyAction();
            _areasToActivate[i].gameObject.SetActive(true);
        }
        _activatingbuyingArea.gameObject.SetActive(false);
    }
}
