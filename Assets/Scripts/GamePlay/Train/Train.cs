using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public event Action OnAllPassengersOnSeat, OnAllNeedsGet;

    private const int TRAIN_LENGTH_HALF = 14;
    [SerializeField] private AllTrain _allTrain;
    [SerializeField] private Seat[] _seats;
    [SerializeField] private AIMoveThrowAreas[] _passengerPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private InteraciveArea[] _cashRegister;
    [SerializeField] private InteraciveArea _exitPoint;
    [SerializeField] private CanGiveArea[] _needAreasInTheTrain;
    [SerializeField] private Door[] _doorAutomatic;
    [SerializeField] private Door _enterDoor;
    [SerializeField] private DoorOpener _doorOpener;

    public List<CanGiveArea> _currentCanGiveArea = new List<CanGiveArea>();
    private List<CarringObjectType> _avialableNeed = new List<CarringObjectType>();
    private List<TakeNeedArea> _currentTakeNeedArea = new List<TakeNeedArea>();
    private List<CleanArea> _currentToCleanArea = new List<CleanArea>();
    private List<AIMoveThrowAreas> _currentPassengers = new List<AIMoveThrowAreas>();
    private int _seatsCount;
    private int _currenIndexCashRegister;

    public List<CarringObjectType> AvialableNeed => _avialableNeed;
    public List<TakeNeedArea> CurrentTakeNeedArea => _currentTakeNeedArea;
    public List<CleanArea> CurrentToCleanArea => _currentToCleanArea;
    public List<CanGiveArea> CurrentCanGiveArea => _currentCanGiveArea;

    private void Awake()
    {
        InitializeSeatsCount();
        InitializeAvaliableNeed();
        InitializeAvaliableCanGiveArea();
    }
    private void Start()
    {
        GameEvents.instance.OnAddedNewNeed += OnAddNewNeed;
        GameEvents.instance.OnDisableNeed += OnDisableNeed;
        GameEvents.instance.OnAddedToClean += OnAddNewClean;
        GameEvents.instance.OnCleanArea += OnGetClean;
        GameEvents.instance.OnTrainStartMove += OnStartMove;
        GameEvents.instance.OnTrainArrived += OnArrivedToStation;

        _enterDoor.OnDoorOpened += OnOpenEnterDoor;

        if (_allTrain._isOnStation)
        {
            OnArrivedToStation();
        }
        else if (_currentTakeNeedArea.Count <= 0)
        {
            OnAllNeedsGet?.Invoke();
        }
    }
    private void OnDestroy()
    {
        GameEvents.instance.OnAddedNewNeed -= OnAddNewNeed;
        GameEvents.instance.OnDisableNeed -= OnDisableNeed;
        GameEvents.instance.OnAddedToClean -= OnAddNewClean;
        GameEvents.instance.OnCleanArea -= OnGetClean;
        GameEvents.instance.OnTrainStartMove -= OnStartMove;
        GameEvents.instance.OnTrainArrived -= OnArrivedToStation;

        _enterDoor.OnDoorOpened -= OnOpenEnterDoor;

        StopAllCoroutines();
    }
    private void InitializeSeatsCount()
    {
        for (int i = 0; i < _seats.Length; i++)
        {
            _seatsCount += _seats[i].SeatPlace.Count;
        }
    }
    public void SpawnPassengers()
    {
        StartCoroutine(SpawnPassangersWithDelay(_spawnDelay, _spawnDelay));
    }
    IEnumerator SpawnPassangersWithDelay(float preSpawnDelay, float everyUnitSpawnDelay)
    {
        yield return new WaitForSeconds(preSpawnDelay);

        _currentPassengers.Clear();
        for (int i = 0; i < _seatsCount;)
        {
            for (int j = 0; j < _cashRegister.Length; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, _passengerPrefab.Length * 5) % _passengerPrefab.Length;
                CanGiveArea freeSeat = GetFreeSeat(out int seatCost);
                if (freeSeat == null)
                {
                    i++;
                    break;
                }
                //AIMoveThrowAreas bufPassanger = Instantiate(_passengerPrefab[randomIndex], _spawnPoint.position - _spawnPoint.forward * i * 0.5f, _spawnPoint.rotation, _spawnPoint);
                AIMoveThrowAreas bufPassanger = Instantiate(_passengerPrefab[randomIndex], _spawnPoint.position, _spawnPoint.rotation, _spawnPoint);

                bufPassanger.SetMainWaypoints(_cashRegister[_currenIndexCashRegister], freeSeat, _exitPoint);
                bufPassanger.CurrentSeatCost = seatCost;
                _currenIndexCashRegister++;
                _currenIndexCashRegister %= _cashRegister.Length;

                bufPassanger.Train = this;

                _currentPassengers.Add(bufPassanger);

                i++;

                if (i >= _seatsCount)
                {
                    _currentPassengers[_currentPassengers.Count - 1].OnGetSeatArea += OnLastPassengerGetSeat;
                    break;
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        _doorOpener.gameObject.SetActive(true);
    }
    public void OnStartMove()
    {
        CloseBackDoor();
        _enterDoor.Close();
    }
    public void OnArrivedToStation()
    {
        OpenBackDoor();

        _currentTakeNeedArea.Clear();
        StartCoroutine(OnTrainArrivedCoroutine(_spawnDelay));
        StartCoroutine(SpawnPassangersWithDelay(_spawnDelay * _seats.Length + 1, _spawnDelay));
    }

    IEnumerator OnTrainArrivedCoroutine(float delay)
    {
        for (int i = _seats.Length - 1; i >= 0; i--)
        {
            _seats[i].OnTrainArrived();
            yield return new WaitForSeconds(delay);
        }
    }

    //----------------------//

    private CanGiveArea GetFreeSeat(out int seatCost)
    {
        for (int i = 0; i < _seats.Length; i++)
        {
            if (_seats[i].IsGetFreeSeat(out CanGiveArea freeSeat))
            {
                seatCost = _seats[i].GetMoneyForTicket();
                return freeSeat;
            }
        }
        seatCost = 0;
        return null;
    }
    //------------------------------//
    private void InitializeAvaliableNeed()
    {
        for (int i = 0; i < _needAreasInTheTrain.Length; i++)
        {
            if (_needAreasInTheTrain[i].gameObject.activeInHierarchy && !_avialableNeed.Contains(_needAreasInTheTrain[i].ObjectGive.CarringObjectType))
            {
                _avialableNeed.Add(_needAreasInTheTrain[i].ObjectGive.CarringObjectType);
            }
            _needAreasInTheTrain[i].OnEnabledOnHierarchy += OnBuyNewNeedArea;
        }
    }
    private void OnBuyNewNeedArea(CanGiveArea canGiveArea)
    {
        canGiveArea.OnEnabledOnHierarchy -= OnBuyNewNeedArea;
        if (!_avialableNeed.Contains(canGiveArea.ObjectGive.CarringObjectType))
        {
            _avialableNeed.Add(canGiveArea.ObjectGive.CarringObjectType);
        }
    }

    public CanGiveArea GetGiveAreaByType(CarringObjectType objectType)
    {
        CanGiveArea canGiveArea = null;
        for (int i = 0; i < _currentCanGiveArea.Count; i++)
        {
            if (_currentCanGiveArea[i].gameObject.activeInHierarchy && _currentCanGiveArea[i].ObjectGive.CarringObjectType == objectType)
            {
                canGiveArea = _currentCanGiveArea[i];
                break;
            }
        }
        return canGiveArea;
    }
    private void InitializeAvaliableCanGiveArea()
    {
        GetComponentsInChildren(true, _currentCanGiveArea);
    }
    //-------------------------------//
    private void OnAddNewNeed(TakeNeedArea takeNeedArea)
    {
        if (takeNeedArea.transform.position.z < transform.position.z + TRAIN_LENGTH_HALF && takeNeedArea.transform.position.z > transform.position.z - TRAIN_LENGTH_HALF)
        {
            _currentTakeNeedArea.Add(takeNeedArea);
        }
    }
    private void OnDisableNeed(TakeNeedArea takeNeedArea)
    {
        if (_currentTakeNeedArea.Contains(takeNeedArea))
        {
            _currentTakeNeedArea.Remove(takeNeedArea);
            if (!_allTrain._isOnStation && _currentTakeNeedArea.Count <= 0)
            {
                OnAllNeedsGet?.Invoke();
            }
        }
    }
    private void OnAddNewClean(CleanArea cleanArea)
    {
        if (cleanArea.transform.position.z < transform.position.z + TRAIN_LENGTH_HALF && cleanArea.transform.position.z > transform.position.z - TRAIN_LENGTH_HALF)
        {
            _currentToCleanArea.Add(cleanArea);
        }
    }
    private void OnGetClean(CleanArea cleanArea)
    {
        if (_currentToCleanArea.Contains(cleanArea))
        {
            _currentToCleanArea.Remove(cleanArea);
        }
    }
    private void OpenBackDoor()
    {
        for (int i = 0; i < _doorAutomatic.Length; i++)
        {
            _doorAutomatic[i].Open();
        }
    }
    private void CloseBackDoor()
    {
        for (int i = 0; i < _doorAutomatic.Length; i++)
        {
            _doorAutomatic[i].Close();
        }
    }
    private void OnOpenEnterDoor()
    {
        for (int i = 0; i < _currentPassengers.Count; i++)
        {
            _currentPassengers[i].MoveToCurrentWaypoint(0.5f); //////////////////////////////
        }
    }

    private void OnLastPassengerGetSeat()
    {
        _currentPassengers[_currentPassengers.Count - 1].OnGetSeatArea -= OnLastPassengerGetSeat;
        OnAllPassengersOnSeat?.Invoke();

        StartCoroutine(CheckNoNeedsLeftDelay(0.5f));
    }

    private IEnumerator CheckNoNeedsLeftDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_currentTakeNeedArea.Count <= 0)
        {
            OnAllNeedsGet?.Invoke();
        }
    }
}
