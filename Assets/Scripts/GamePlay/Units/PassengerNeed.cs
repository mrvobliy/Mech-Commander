using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerNeed : MonoBehaviour
{
    [SerializeField] TakeNeedArea _takeNeedArea;
    [SerializeField] NeedCanvas _needCanvas;
    [SerializeField] AIMoveThrowAreas _aiMoveThrowAreas;

    private void Start()
    {
        _aiMoveThrowAreas.OnGetSeatArea += OnSetNeed;
        _aiMoveThrowAreas.OnTrainArrived += DisableNeed;
    }
    private void OnDestroy()
    {
        DisableNeed();
    }
    private void OnSetNeed()
    {
        _aiMoveThrowAreas.OnGetSeatArea -= OnSetNeed;

        RandomSeed.Time();
        int randomChanceToSetNeed = Random.Range(0, 100);


        if (_aiMoveThrowAreas.Train.AvialableNeed.Count <= 0 || randomChanceToSetNeed > 70)
        {
            return;
        }

        _takeNeedArea.gameObject.SetActive(true);
        _needCanvas.gameObject.SetActive(true);

        RandomSeed.Time();
        int randomNeedIndex = Random.Range(0, _aiMoveThrowAreas.Train.AvialableNeed.Count * 5) % _aiMoveThrowAreas.Train.AvialableNeed.Count;
        CarringObjectType addedNeed = _aiMoveThrowAreas.Train.AvialableNeed[randomNeedIndex];
        _takeNeedArea.AddNeed(addedNeed);
        _needCanvas.OnEnableNeed(addedNeed);

        _takeNeedArea.OnGetNeed += OnGetNeed;
        _takeNeedArea.OnDoSomething += OnDoSomething;

        GameEvents.instance.AddedNewNeed(_takeNeedArea);
    }

    private void OnDoSomething()
    {
        DisableNeed();
    }
    private void OnGetNeed(int cost)
    {
        _takeNeedArea.OnGetNeed -= OnGetNeed;
        MoneyPayer.Pay(cost, transform.position, new Vector3(0f, transform.position.y, transform.position.z));
    }

    private void DisableNeed()
    {
        _takeNeedArea.OnDoSomething -= OnDoSomething;
        _aiMoveThrowAreas.OnTrainArrived -= DisableNeed;
        _takeNeedArea.DoSomethingAction(); ///////////////////////////

        _takeNeedArea.gameObject.SetActive(false);
        _needCanvas.gameObject.SetActive(false);
    }
}
