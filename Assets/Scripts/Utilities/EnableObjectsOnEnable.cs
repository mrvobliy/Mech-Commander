using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectsOnEnable : MonoBehaviour
{
    [SerializeField] private GameObject[] _setActiveOnEnable;
    private bool _isUsed;

    private void Start()
    {
        if (_isUsed)
        {
            return;
        }

        _isUsed = true;
        if(_setActiveOnEnable.Length > 0)
        {
            for(int i = 0; i < _setActiveOnEnable.Length; i++)
            {
                _setActiveOnEnable[i].SetActive(true);
            }
        }
    }
}
