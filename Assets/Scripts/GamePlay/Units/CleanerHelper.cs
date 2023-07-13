using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CleanerHelper : MonoBehaviour
{
    public InteraciveArea[] _waypoints = new InteraciveArea[2];

    [SerializeField] protected Transform _unitVisual;
    [SerializeField] protected Train _train;
    [SerializeField] private CanGiveArea _startPoint;

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
        GameEvents.instance.OnAddedToClean += OnAddedNewObjectToClean;

        _waypoints[0] = _startPoint;
        if (IsStartHelp())
        {
            _isHelp = true;
            _currentWaypoint = 1;
            MoveToCurrentWaypoint();
        }
    }
    private void OnDestroy()
    {
        GameEvents.instance.OnAddedToClean -= OnAddedNewObjectToClean;
        _waypoints[_currentWaypoint].OnDoSomething -= OnAreaCleaned;

        StopAllCoroutines();

    }

    public void MoveToCurrentWaypoint()
    {
        if (_currentWaypoint < _waypoints.Length)
        {
            _navMeshAgent.SetDestination(_waypoints[_currentWaypoint].transform.position);
            if (_waitForStopCoroutine != null)
            {
                StopCoroutine(_waitForStopCoroutine);
            }
            _waitForStopCoroutine = StartCoroutine(WaitForAgentStop());

            _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Walk));
        }
    }

    protected void OnAreaCleaned()
    {
        _waypoints[_currentWaypoint].OnDoSomething -= OnAreaCleaned;

        _currentWaypoint = 0;
        _isHelp = false;
        if (IsStartHelp())
        {
        }
        MoveToCurrentWaypoint();
    }

    private bool IsStartHelp()
    {
        if(Train.CurrentToCleanArea.Count > 0)
        {
            _waypoints[1] = Train.CurrentToCleanArea[0];
            _waypoints[1].OnDoSomething += OnAreaCleaned;
            _isHelp = true;
            _currentWaypoint = 1;
            return true;
        }
        
        return false;
    }
    private void OnAddedNewObjectToClean(CleanArea cleanArea)
    {
        if (_isHelp)
        {
            return;
        }

        if (IsStartHelp())
        {
            _isHelp = true;
            _currentWaypoint = 1;
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
