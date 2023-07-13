using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casher : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DoorOpener>(out DoorOpener doorOpener))
        {
            _animator.SetTrigger(AnimationNames.GetAnimationStringName(AnimationNamesEnum.PushButton));
        }
    }
}
