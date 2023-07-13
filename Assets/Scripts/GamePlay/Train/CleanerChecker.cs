using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerChecker : MonoBehaviour
{
    [SerializeField] private CleanObject[] _cleanObject;
    [SerializeField] private GameObject _cleanerEnabler;

    private int _alreadyCleanedObject;

    private void Start()
    {
        GameEvents.instance.OnTrainStartMove += OnTrainStartMove;
        for(int i = 0; i < _cleanObject.Length; i++)
        {
            _cleanObject[i].OnSetClean += OnCleanedObject;
        }
    }
    private void OnDestroy()
    {
        GameEvents.instance.OnTrainStartMove -= OnTrainStartMove;
        for (int i = 0; i < _cleanObject.Length; i++)
        {
            _cleanObject[i].OnSetClean -= OnCleanedObject;
        }
    }

    private void OnCleanedObject()
    {
        _alreadyCleanedObject++;
        if (_alreadyCleanedObject == _cleanObject.Length)
        {
            _alreadyCleanedObject = 0;
            _cleanerEnabler.SetActive(true);
        }
    }
    private void OnTrainStartMove()
    {
        _cleanerEnabler.SetActive(false);
    }
}
