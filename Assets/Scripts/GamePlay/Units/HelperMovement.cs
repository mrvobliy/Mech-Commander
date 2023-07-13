using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class HelperMovement : MonoBehaviour
{
    public InteraciveArea[] _waypoints = new InteraciveArea[3];

    [SerializeField] protected Transform _unitVisual;
    [SerializeField] protected Train _train;
    [SerializeField] private List<CarringObjectType> _carringObjectCanGive = new List<CarringObjectType>();
    [SerializeField] private CanGiveArea _startPoint;
    [SerializeField] private StackObject _stackObject;
    [SerializeField] private CanTakeArea _recycler; /////////////////////// need to get from train

    protected int _currentWaypoint = 0;
    protected Unit _unit;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Coroutine _walkTimerCoroutine;
    protected bool _isHelp;
    protected Coroutine _waitForStopCoroutine;
    protected int _frameOnEventCatched;

    public Train Train { get { return _train; } set { _train = value; } }

    protected void Awake()
    {
        _unit = GetComponent<Unit>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        GameEvents.instance.OnAddedNewNeed += OnAddNewNeedArea;
        _stackObject.OnStackCountChanged += OnStackObjectCountChanged;

        _waypoints[0] = _startPoint;
        if (IsCanHelp())
        {
            _isHelp = true;
            _currentWaypoint++;
            MoveToCurrentWaypoint();
        }
    }
    private void OnDestroy()
    {
        _stackObject.OnStackCountChanged -= OnStackObjectCountChanged;
        GameEvents.instance.OnAddedNewNeed -= OnAddNewNeedArea;

        if (_waitForStopCoroutine != null)
        {
            StopCoroutine(_waitForStopCoroutine);
        }
        if (_walkTimerCoroutine != null)
        {
            StopCoroutine(_walkTimerCoroutine);
        }
        
    }

    public void MoveToCurrentWaypoint()
    {
        if (_currentWaypoint < _waypoints.Length && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.SetDestination(_waypoints[_currentWaypoint].transform.position);
            if(_waitForStopCoroutine != null)
            {
                StopCoroutine(_waitForStopCoroutine);
            }
            _waitForStopCoroutine = StartCoroutine(WaitForAgentStop());

            _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Walk));
        }
    }
    public void MoveToWaypoint(CanTakeArea canTakeArea)
    {
        if (canTakeArea != null && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.SetDestination(canTakeArea.transform.position);
            if (_waitForStopCoroutine != null)
            {
                StopCoroutine(_waitForStopCoroutine);
            }
            _waitForStopCoroutine = StartCoroutine(WaitForAgentStop());

            _unit.IsMove = true;
            _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Walk));
        }
    }

    protected void OnStackObjectCountChanged(int value)
    {
        OnEventCatched();
    }
    protected void OnAreaGetNeedObject()
    {
        _waypoints[_currentWaypoint].OnDoSomething -= OnAreaGetNeedObject;
        if(_currentWaypoint == 2)
        {
            MoveToWaypoint(_recycler);
        }
        else
        {
            _currentWaypoint = 0;
            _isHelp = false;
            if (IsCanHelp())
            {
                _isHelp = true;
                _currentWaypoint++;
            }
            MoveToCurrentWaypoint();
        }
    }
    private void OnEventCatched()
    {
        if(_frameOnEventCatched == Time.frameCount)
        {
            return;
        }
        _frameOnEventCatched = Time.frameCount;

        switch (_currentWaypoint)
        {
            case 0:
                _isHelp = true;
                _currentWaypoint++;
                MoveToCurrentWaypoint();
                break;

            case 1:
                _currentWaypoint++;
                MoveToCurrentWaypoint();
                break;

            case 2:
                _waypoints[2].OnDoSomething -= OnAreaGetNeedObject;
                _currentWaypoint = 0;
                _isHelp = false;
                if (IsCanHelp())
                {
                    _isHelp = true;
                    _currentWaypoint++;
                }

                MoveToCurrentWaypoint();
                break;

            default:
                break;
        }
    }

    private bool IsCanHelp()
    {
        bool isMatchWithTrainAvialableNeed = false;

        for(int i = 0; i < Train.CurrentTakeNeedArea.Count; i++)
        {
            isMatchWithTrainAvialableNeed = Train.CurrentTakeNeedArea[i].gameObject.activeInHierarchy &&  _carringObjectCanGive.Contains(Train.CurrentTakeNeedArea[i].NeedObjects[0]);///////////       0        //////////////////////////////
            if (isMatchWithTrainAvialableNeed)
            {
                _waypoints[1] = Train.GetGiveAreaByType(Train.CurrentTakeNeedArea[i].NeedObjects[0]);
                _waypoints[2] = Train.CurrentTakeNeedArea[i];
                _waypoints[2].OnDoSomething += OnAreaGetNeedObject;
                break;
            }
        }

        return isMatchWithTrainAvialableNeed;
    }
    private void OnAddNewNeedArea(TakeNeedArea newNeedArea)
    {
        if (_isHelp)
        {
            return;
        }

        if (IsCanHelp())
        {
            _isHelp = true;
            _currentWaypoint++;
            MoveToCurrentWaypoint();
        }
    }

    IEnumerator WaitForAgentStop()
    {
        while (_navMeshAgent.pathPending)
        {
            yield return null;
        }
        while (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return new WaitForEndOfFrame();
        }
        _unit.IsMove = false;

        _animator.ResetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Walk));
        _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Idle));
    }
}
