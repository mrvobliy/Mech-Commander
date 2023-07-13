using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleOnEnable : MonoBehaviour
{
    public bool _onlyOnce;
    private bool _scaledAlready;

    public float _fromScale, _toScale, _duration;
    private void OnEnable()
    {
        if(_onlyOnce && _scaledAlready)
        {
            return;
        }

        _scaledAlready = true;
        transform.localScale = Vector3.one * _fromScale;
        transform.DOScale(_toScale, _duration).SetAutoKill(true);
    }
}
