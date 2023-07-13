using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    [SerializeField] TakeNeedArea _takeNeedArea;
    [SerializeField] NeedCanvas _needCanvas;
    [SerializeField] GameObject _lockingStripe;

    private bool _isFull;

    private void Start()
    {
        if (_isFull)
        {
            return;
        }

        OnSetNeed();
    }
    private void OnDestroy()
    {
        DisableNeed();
    }
    private void OnSetNeed()
    {
        _isFull = true;

        _lockingStripe.SetActive(true);
        _takeNeedArea.gameObject.SetActive(true);
        _needCanvas.gameObject.SetActive(true);

        CarringObjectType addedNeed = CarringObjectType.ToiletPaper;
        _takeNeedArea.AddNeed(addedNeed);
        _needCanvas.OnEnableNeed(addedNeed);

        _takeNeedArea.OnDoSomething += DisableNeed;

        GameEvents.instance.AddedNewNeed(_takeNeedArea);
    }

    private void OnDoSomething()
    {
        DisableNeed();
    }

    private void DisableNeed()
    {
        _takeNeedArea.OnDoSomething -= OnDoSomething;

        _lockingStripe.SetActive(false);
        _takeNeedArea.gameObject.SetActive(false);
        _needCanvas.gameObject.SetActive(false);
    }
}
