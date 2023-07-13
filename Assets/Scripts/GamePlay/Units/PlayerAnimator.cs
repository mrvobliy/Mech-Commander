using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private StackObject _stackObject;
    private void Start()
    {
        _stackObject.OnStackCountChanged += OnChangeStackCount;
    }
    private void OnDestroy()
    {
        _stackObject.OnStackCountChanged -= OnChangeStackCount;
    }

    private void OnChangeStackCount(int value)
    {
        if(value <= 0)
        {
            _playerAnimator.SetBool("Carry", false);
            return;
        }
        _playerAnimator.SetBool("Carry", true);
    }
}
