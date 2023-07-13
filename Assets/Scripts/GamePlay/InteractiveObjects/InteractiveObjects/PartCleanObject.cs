using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCleanObject : MonoBehaviour
{
    [SerializeField] private GameObject _cleanVisual, _dirtyVisual;
    public void SetClean(bool isClean)
    {
        _cleanVisual.SetActive(isClean);
        _dirtyVisual.SetActive(!isClean);
    }
}
