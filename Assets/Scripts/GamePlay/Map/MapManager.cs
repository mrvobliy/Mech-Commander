using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject _mapCanvas;
    [SerializeField] private MapOfAmerica _mapOfAmerica;
    [SerializeField] private float _mapShowingTime;

    private void Start()
    {
        GameEvents.instance.OnTrainArrived += OnArrived;
    }
    private void OnDestroy()
    {
        GameEvents.instance.OnTrainArrived -= OnArrived;
        StopAllCoroutines();
    }

    private void OnArrived()
    {
        _mapCanvas.SetActive(true);
        StartCoroutine(ShowingTimer());
    }

    IEnumerator ShowingTimer()
    {
        yield return new WaitForSeconds(_mapShowingTime / 2f);

        _mapOfAmerica.ActivateCurrentState();

        yield return new WaitForSeconds(_mapShowingTime);

        HideMap();
    }

    public void HideMap()
    {
        _mapCanvas.SetActive(false);
    }
}
