using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectOnEnable : MonoBehaviour
{
    [SerializeField] private GameObject[] _setActiveOnEnable;

    private void OnEnable()
    {
        if (_setActiveOnEnable.Length > 0)
        {
            for (int i = 0; i < _setActiveOnEnable.Length; i++)
            {
                _setActiveOnEnable[i].SetActive(false);
            }
        }
    }
}
