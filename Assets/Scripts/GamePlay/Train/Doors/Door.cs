using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public event Action OnDoorOpened;

    private Tween _moveTween;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }
    public void Open()
    {
        if (_moveTween.IsActive())
        {
            _moveTween.Kill();
        }
        _moveTween = transform.DOMove(_startPosition + transform.right * 1.25f + transform.forward * 0.3f, 0.5f).SetDelay(1f).OnComplete(() => OnDoorOpened?.Invoke()).SetAutoKill(true);
    }
    public void Close()
    {
        if (_moveTween.IsActive())
        {
            _moveTween.Kill();
        }
        _moveTween = transform.DOMove(_startPosition, 0.5f).SetAutoKill(true);
    }
}
