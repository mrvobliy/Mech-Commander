using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private const float ROTATION_SPEED = 10f;

    public Joystick _joystick;
    public NavMeshAgent _agent;

    private Unit _unit;
    private Animator _animator;
    private bool _isWalk;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        bool bufWalkBool = false;

        Vector2 input = _joystick.Direction;
        Vector3 dirrection = transform.forward;
        if (input.magnitude > 0.1f)
        {
            bufWalkBool = true;

            Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
            Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

            dirrection = rightVector * input.x + forwardVector * input.y;
            _agent.Move(dirrection.normalized * _agent.speed * Time.deltaTime);
        }

        if(_isWalk != bufWalkBool)
        {
            _isWalk = bufWalkBool;

            if (bufWalkBool)
            {
                _unit.IsMove = true;
                _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Walk));
            }
            else
            {
                _unit.IsMove = false;
                _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.Idle));
            }
        }

        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            Vector3 tangent = Vector3.Cross(_agent.transform.up, _agent.desiredVelocity.normalized);
            Vector3 targetPosition = _agent.transform.position + tangent * 2f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 2f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }

        Quaternion lookatQuaternion = Quaternion.LookRotation(dirrection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookatQuaternion, Time.deltaTime * ROTATION_SPEED);
    }
}
