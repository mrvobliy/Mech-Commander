using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum InteractiveType
{
    Buying,
    CanGiveArea,
    CanTakeArea,
    CustomerArea,
    CanActivateArea,
    Cleaning
}

public class InteraciveArea : MonoBehaviour
{
    public event Action OnDoSomething, OnEnterInTrigger;

    [SerializeField] protected GameObject _visual;
    [SerializeField] protected Transform _interactivePoint;
    [SerializeField] protected float _timeToCallEvent;
    [SerializeField] protected AnimationNamesEnum _enterAnimationTrigger;
    [SerializeField] protected bool _isHasQueue;
    [SerializeField] protected bool _isSaveInteractiveProgress;

    protected InteractiveType _interactiveType;
    protected List<Unit> _currentUnit = new List<Unit>();
    protected Tween _scaleTween;

    protected float _currentTimeToCallEvent;
    protected bool _isReserved;

    public bool IsReserved { get { return _isReserved; } set { _isReserved = value; } }
    public bool IsHasQueue { get { return _isHasQueue; } set { _isHasQueue = value; } }
    public Transform InteractivePoint => _interactivePoint;
    public InteractiveType InteractiveType => _interactiveType;
    public AnimationNamesEnum EnterAnimationTrigger => _enterAnimationTrigger;

    protected virtual void OnDisable()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Unit>(out Unit curUnit) && curUnit.InteractiveListType.Contains(_interactiveType) && !_currentUnit.Contains(curUnit))
        {
            _currentUnit.Add(curUnit);
            OnTriggerEnterDo();
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        bool isCurrentUserContainCollider = false;
        for(int i = _currentUnit.Count - 1; i >= 0; i--)
        {
            if(_currentUnit[i].gameObject == null)
            {
                _currentUnit.RemoveAt(i);
                continue;
            }
            isCurrentUserContainCollider = isCurrentUserContainCollider || _currentUnit[i].gameObject == other.gameObject;
        }

        if (_currentUnit.Count > 0 && isCurrentUserContainCollider)
        {
            if (!_scaleTween.IsActive())
            {
                _scaleTween = _visual.transform.DOScale(1.2f, 0.35f).SetAutoKill(true);
            }
            OnTriggeredUnit(_currentUnit);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Unit>(out Unit curUnit) && _currentUnit.Contains(curUnit))
        {
            _currentUnit.Remove(curUnit);
        }
        if (_currentUnit.Count <= 0)
        {
            OnTriggerExitDo();
            if (_scaleTween.IsActive())
            {
                _scaleTween.Kill();
            }
            _scaleTween = _visual.transform.DOScale(1f, 0.35f).SetAutoKill(true);

            if (!_isSaveInteractiveProgress)
            {
                _currentTimeToCallEvent = 0;
            }
        }
    }

    public virtual void OnTriggeredUnit(List<Unit> unit)
    {
        _currentTimeToCallEvent += Time.deltaTime;
        if (_currentTimeToCallEvent >= _timeToCallEvent)
        {
            OnDoSomething?.Invoke();

            _currentTimeToCallEvent = 0f;
            for(int i = 0; i < unit.Count; i++)
            {
                OnValideTriggerAction(unit[i]);
            }
        }
    }

    public void OnCurentUnitDestroy(Unit unit)
    {
        if (_currentUnit.Contains(unit))
        {
            _currentUnit.Remove(unit);
        }
    }

    protected virtual void OnValideTriggerAction(Unit unit)
    {

    }

    protected virtual void OnTriggerExitDo()
    {

    }
    protected virtual void OnTriggerEnterDo()
    {
        OnEnterInTrigger?.Invoke();
    }

    public void DoSomethingAction()
    {
        OnDoSomething?.Invoke();
    }
}
