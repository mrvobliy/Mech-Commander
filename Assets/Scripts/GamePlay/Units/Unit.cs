using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] protected List<InteractiveType> _interactiveListType = new List<InteractiveType>();
    [SerializeField] protected List<UnitModule> _unitModule = new List<UnitModule>();
    [SerializeField] protected Animator _animator;

    private NavMeshAgent _navmeshAgent;
    private bool _isMove;

    public bool IsMove { get { return _isMove; } set { _isMove = value; } }
    public NavMeshAgent NavMeshAgent => _navmeshAgent;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _navmeshAgent = GetComponent<NavMeshAgent>();
    }

    public List<InteractiveType> InteractiveListType { get { return _interactiveListType; } set { _interactiveListType = value; } }
    public List<UnitModule> UnitModuleList { get { return _unitModule; } set { _unitModule = value; } }

    public void PlayAnimationByTriggerOnTranform(AnimationNamesEnum animationEnum, Transform targetTransform)
    {
        transform.rotation = targetTransform.rotation;
        _animator.SetTrigger(AnimationNames.GetAnimationStringName(animationEnum));
    }

}
