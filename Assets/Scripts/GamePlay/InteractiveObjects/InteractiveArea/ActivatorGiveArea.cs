using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActivatorGiveArea : InteraciveArea
{
    [SerializeField] private CanGiveArea[] _cangiveArea;
    private void Awake()
    {
        _interactiveType = InteractiveType.CanActivateArea;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(_currentUnit.Count > 0)
        {
            if (!_scaleTween.IsActive())
            {
                _scaleTween = _visual.transform.DOScale(1.2f, 0.35f).SetAutoKill(true);
            }
            ActivateArea();
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        
    }
    protected override void OnTriggerExitDo()
    {
        if (_currentUnit.Count <= 0)
        {
            DeactivateArea();
        }
    }

    private void ActivateArea()
    {
        for(int i = 0; i < _cangiveArea.Length; i++)
        {
            _cangiveArea[i].SetObjectNumber(-1);
        }
    }

    private void DeactivateArea()
    {
        for (int i = 0; i < _cangiveArea.Length; i++)
        {
            _cangiveArea[i].SetObjectNumber(0);
        }
    }
}
