using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapOfAmerica : MonoBehaviour
{
    private Image[] _stateImage;
    private int _currentIndex;

    private void Awake()
    {
        _stateImage = GetComponentsInChildren<Image>(true);

        for(int i = 0; i < _stateImage.Length; i++)
        {
            _stateImage[i].gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        for (int i = 0; i < _currentIndex; i++)
        {
            _stateImage[i].gameObject.SetActive(true);
        }
    }

    public void ActivateCurrentState()
    {
        ActivateStateByIndex(_currentIndex);
        _currentIndex++;
        _currentIndex %= _stateImage.Length;
    }
    private void ActivateStateByIndex(int index)
    {
        _stateImage[index].gameObject.SetActive(!_stateImage[index].gameObject.activeInHierarchy);
    }
}
