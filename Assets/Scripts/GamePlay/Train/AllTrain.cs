using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Tabtale.TTPlugins;

public class AllTrain : MonoBehaviour
{
    [Header("Trains")]
    public Train[] _train;
    public StationMove _station;

    [Header("Movement")]
    public float _maxSpeed;
    public float _accelerationTime;
    public EnvironmentMove[] _moveObjects;
    public TrainShake _trainShake;

    private float _currentSpeed;
    private Tween _accelerationTween;
    private int _readyToGoTrains, _readyToStopTrains;

    [Header("Else")]
    public Unit _playerUnit;
    public CinemachineVirtualCamera _virtualCamera;
    public bool _isOnStation;
    private void Awake()
    {
        TTPCore.Setup();

        _isOnStation = true;
    }
    private void Start()
    {
        for(int i = 0; i < _train.Length; i++)
        {
            _train[i].OnAllPassengersOnSeat += OnPassengersOnTrainOnSeat;
            _train[i].OnAllNeedsGet += OnTrainReadyToStop;
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < _train.Length; i++)
        {
            _train[i].OnAllPassengersOnSeat -= OnPassengersOnTrainOnSeat;
            _train[i].OnAllNeedsGet -= OnTrainReadyToStop;
        }
    }
    public void StartMove()
    {
        _isOnStation = false;
        GameEvents.instance.TrainStartMove();
        if (_accelerationTween.IsActive())
        {
            _accelerationTween.Kill();
        }
        _accelerationTween = DOVirtual.Float(_currentSpeed, _maxSpeed, _accelerationTime, SetAllTrainSpeed).OnComplete(() => MaxSpeedReached() ).SetAutoKill(true);
    }
    private void MaxSpeedReached()
    {
        OnTrainReadyToStop();
    }
    public void StopMove()
    {
        ShowStation();

        if (_accelerationTween.IsActive())
        {
            _accelerationTween.Kill();
        }
        _accelerationTween = DOVirtual.Float(_currentSpeed, 0f, _accelerationTime, SetAllTrainSpeed).OnComplete(() => Arrived()).SetAutoKill(true);
    }
    public void Arrived()
    {
        _isOnStation = true;
        GameEvents.instance.TrainArrived();
        _trainShake.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // --------------------------------- // -------------------------------------- //
    private void SetAllTrainSpeed(float currentSpeed)
    {
        _currentSpeed = currentSpeed;
        for (int i = 0; i < _moveObjects.Length; i++)
        {
            _moveObjects[i]._speed = currentSpeed;
        }
        _trainShake._rotationSpeed = currentSpeed / 10f;

        CinemachineZoomFromSpeed(currentSpeed);
    }
    private void ShowStation()
    {
        float stationZposition = _currentSpeed * _accelerationTime * 0.332f;
        Vector3 stationPosition = Vector3.forward * stationZposition;

        _station.gameObject.SetActive(true);
        _station.transform.position = stationPosition;
    }

    private void OnPassengersOnTrainOnSeat()
    {
        _readyToGoTrains++;

        int activeTrainNumber = 0;
        for(int i = 0; i < _train.Length; i++)
        {
            if (_train[i].gameObject.activeInHierarchy)
            {
                activeTrainNumber++;
            }
        }

        if(_readyToGoTrains == activeTrainNumber)
        {
            _readyToGoTrains = 0;
            StartCoroutine(WaitForPlayerUnitInTrain());
        }
    }
    IEnumerator WaitForPlayerUnitInTrain()
    {
        while(_playerUnit.gameObject.transform.position.x > 3.25f)
        {
            yield return null;
        }

        StartMove();
    }
    private void OnTrainReadyToStop()
    {
        _readyToStopTrains++;

        int activeTrainNumber = 0;
        for (int i = 0; i < _train.Length; i++)
        {
            if (_train[i].gameObject.activeInHierarchy)
            {
                activeTrainNumber++;
            }
        }

        if (_readyToStopTrains >= activeTrainNumber + 1)
        {
            _readyToStopTrains = 0;
            StopMove();
        }
    }
    private void CinemachineZoomFromSpeed(float speed)
    {
        float startFow = 60;
        float fowZoomValue = 10;

        _virtualCamera.m_Lens.FieldOfView = startFow + speed * fowZoomValue / _maxSpeed;
    }
}
