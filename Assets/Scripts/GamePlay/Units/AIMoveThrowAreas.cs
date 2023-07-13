using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class AIMoveThrowAreas : MonoBehaviour
{
    public event Action OnGetSeatArea, OnTrainArrived;

    public InteraciveArea[] _waypoints = new InteraciveArea[3];

    [SerializeField] protected Transform _unitVisual;
    [SerializeField] protected Train _train;

    protected Tween _visualTweenMove, _visualTweenRotate;
    protected int _currentWaypoint = 1;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Coroutine _walkTimerCoroutine;
    protected int _currentSeatCost;

    public int CurrentSeatCost { get { return _currentSeatCost; } set { _currentSeatCost = value; } }
    public Train Train { get { return _train; } set { _train = value; } }

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _navMeshAgent.SetDestination(transform.position + transform.forward * 0.5f);
    }

    private void OnDestroy()
    {
        if (_currentWaypoint < _waypoints.Length)
        {
            _waypoints[_currentWaypoint].OnDoSomething -= OnThisDoSomething;
            _waypoints[_currentWaypoint].OnDoSomething -= OnReservedDoSomething;
        }
        transform.DOKill();
        _unitVisual.transform.DOKill();
    }

    public void MoveToCurrentWaypoint(float prewalkDelay)
    {
        if (_currentWaypoint < _waypoints.Length)
        {
            if (_waypoints[_currentWaypoint].IsHasQueue && _waypoints[_currentWaypoint].IsReserved)
            {
                _waypoints[_currentWaypoint].OnDoSomething += OnReservedDoSomething;
            }
            else
            {
                _waypoints[_currentWaypoint].IsReserved = true;
                _waypoints[_currentWaypoint].OnDoSomething += OnThisDoSomething;

                if (_unitVisual.transform.localPosition != Vector3.zero)
                {
                    _visualTweenMove.Kill();
                    _visualTweenMove = _unitVisual.DOLocalMove(Vector3.zero, 0.6f).SetDelay(0.6f).SetAutoKill(true);
                    _visualTweenRotate.Kill();
                    //_unitVisual.localRotation = Quaternion.Euler(Vector3.zero);
                    _visualTweenRotate = _unitVisual.DOLocalRotate(Vector3.zero, 0.6f).SetDelay(0.6f).SetAutoKill(true);
                }

                _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Walk));

                if (_walkTimerCoroutine != null)
                {
                    StopCoroutine(_walkTimerCoroutine);
                }

                _navMeshAgent.isStopped = false;
                _walkTimerCoroutine = StartCoroutine(WalkingTimer(prewalkDelay, _waypoints[_currentWaypoint].transform.position));
            }
        }
    }

    IEnumerator WalkingTimer(float preWalkDelay, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(preWalkDelay);

        _navMeshAgent.SetDestination(targetPosition);
        while (_navMeshAgent.pathPending)
        {
            yield return null;
        }

        Vector3 endPosition = _navMeshAgent.path.corners[_navMeshAgent.path.corners.Length - 1];

        while (Vector3.Distance(transform.position, endPosition) > _navMeshAgent.stoppingDistance)
        {
            yield return new WaitForEndOfFrame();
        }
        
        OnReachDestination();
    }

    protected virtual void OnReachDestination()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetTrigger(AnimationNames.GetAnimationStringName(_waypoints[_currentWaypoint].EnterAnimationTrigger));
        transform.DORotateQuaternion(_waypoints[_currentWaypoint].transform.rotation, 0.25f).SetAutoKill(true);

        _visualTweenMove.Kill();
        _visualTweenMove = _unitVisual.DOMove(_waypoints[_currentWaypoint].InteractivePoint.position, 0.6f).SetAutoKill(true);
        _visualTweenRotate.Kill();
        _visualTweenRotate = _unitVisual.DORotateQuaternion(_waypoints[_currentWaypoint].InteractivePoint.rotation, 0.6f).SetAutoKill(true);
        
        if (_currentWaypoint + 1 >= _waypoints.Length)
        {
            _waypoints[_currentWaypoint].OnCurentUnitDestroy(GetComponent<Unit>());
            Destroy(gameObject);
        }
        if (_currentWaypoint == 1)
        {
            OnGetSeatArea?.Invoke();
        }
    }
    protected virtual void OnThisDoSomething()
    {
        float preWalkDelay = 0;

        if(_currentWaypoint == 1)
        {
            preWalkDelay = 1.2f; //////////////////////////////////////
            OnTrainArrived?.Invoke();
        }

        _currentWaypoint++;
        if (_currentWaypoint < _waypoints.Length)
        {
            if (_currentWaypoint > 0)
            {
                _waypoints[_currentWaypoint - 1].OnDoSomething -= OnThisDoSomething;
                _waypoints[_currentWaypoint - 1].IsReserved = false;
            }
            MoveToCurrentWaypoint(preWalkDelay);
        }
    }
    protected virtual void OnReservedDoSomething()
    {
        if (_currentWaypoint < _waypoints.Length)
        {
            _waypoints[_currentWaypoint].OnDoSomething -= OnReservedDoSomething;
            _navMeshAgent.SetDestination(transform.position + transform.forward * 0.5f);
            MoveToCurrentWaypoint(0f);
        }
    }

    public virtual void SetMainWaypoints(InteraciveArea firstPoint, InteraciveArea secondPoint, InteraciveArea thirdPoint)
    {
        _waypoints[0] = firstPoint;
        _waypoints[1] = secondPoint;
        _waypoints[2] = thirdPoint;
    }
}
