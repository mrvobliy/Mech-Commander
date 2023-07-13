using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressManager : MonoBehaviour
{
    public event Action<int> OnLevelChanged;

    public int _currentLevel;
    public int _currentProgressValue;

    public int[] _everyLevelMaxProgressPoint;
    public int _addPerBuy;

    [SerializeField] private TextMeshProUGUI _tmLevel, _tmCurrentValue;
    [SerializeField] private Image _filler;
    [SerializeField] private Image _star;
    private void Start()
    {
        GameEvents.instance.OnBuyObject += OnBuyObject;

        SetProgressValue(_currentProgressValue);
        SetLevel(_currentLevel);
    }
    private void OnDestroy()
    {
        GameEvents.instance.OnBuyObject -= OnBuyObject;
    }
    private void OnBuyObject(int expValue)
    {
        _currentProgressValue += expValue;

        int remain = _currentProgressValue - _everyLevelMaxProgressPoint[_currentLevel];

        if(remain >= 0)
        {
            if(_currentLevel + 1 >= _everyLevelMaxProgressPoint.Length)
            {
                _currentProgressValue = _everyLevelMaxProgressPoint[_currentLevel];
                return;
            }
            SetLevel(_currentLevel + 1);
            _currentProgressValue = remain;
        }

        SetProgressValue(_currentProgressValue);
    }
    private void SetProgressValue(int value)
    {
        _tmCurrentValue.text = _currentProgressValue.ToString() + "/" + _everyLevelMaxProgressPoint[_currentLevel];

        float fillerValue = (float)_currentProgressValue / _everyLevelMaxProgressPoint[_currentLevel];
        _filler.fillAmount = fillerValue;
    }

    private void SetLevel(int toLevel)
    {
        _currentLevel = toLevel;
        _tmLevel.text = (_currentLevel + 1).ToString();

        _star.transform.DORewind();
        _star.transform.DOPunchScale(Vector3.one, 0.5f, 2).SetAutoKill(true);

        OnLevelChanged?.Invoke(_currentLevel);
    }
}
