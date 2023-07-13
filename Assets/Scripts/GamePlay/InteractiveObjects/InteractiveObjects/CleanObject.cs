using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanObject : MonoBehaviour
{
    public event Action OnSetClean;

    [SerializeField] private CleanArea _cleanArea;
    [SerializeField] private CanGiveArea _seatCanGiveArea;
    [SerializeField] private List<PartCleanObject> _cleanObjects;
    [SerializeField] private ParticleSystem _cleanerEffect;

    private void Start()
    {
        _cleanArea.OnDoSomething += OnCleaned;
        _seatCanGiveArea.OnDoSomething += OnDirty;
    }
    private void OnDestroy()
    {
        _cleanArea.OnDoSomething -= OnCleaned;
        _seatCanGiveArea.OnDoSomething -= OnDirty;
    }
    private void OnCleaned()
    {
        for(int i = 0; i< _cleanObjects.Count; i++)
        {
            _cleanObjects[i].SetClean(true);
        }
        OnSetClean?.Invoke();
        GameEvents.instance.CleanArea(_cleanArea);
        _cleanArea.gameObject.SetActive(false);
        _cleanerEffect.Play();
    }
    private void OnDirty()
    {
        _cleanArea.gameObject.SetActive(true);
        for (int i = 0; i < _cleanObjects.Count; i++)
        {
            _cleanObjects[i].SetClean(false);
        }
    }
}
