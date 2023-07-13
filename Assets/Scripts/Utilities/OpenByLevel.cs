using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenByLevel : MonoBehaviour
{
    [SerializeField] private GameObject _objectToOpen;
    [SerializeField] private int _levelIndex;
    [SerializeField] private ProgressManager _progressManager;

    private void Start()
    {
        _progressManager.OnLevelChanged += OnLevelChanged;
    }
    private void OnDestroy()
    {
        _progressManager.OnLevelChanged -= OnLevelChanged;
    }
    private void OnLevelChanged(int level)
    {
        if(level + 1 == _levelIndex)
        {
            _objectToOpen.SetActive(true);
            _progressManager.OnLevelChanged -= OnLevelChanged;
        }
    }
}
