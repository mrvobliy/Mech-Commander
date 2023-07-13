using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public event Action OnTrainArrived;
    public void TrainArrived()
    {
        OnTrainArrived?.Invoke();
    }

    public event Action OnTrainStartMove;
    public void TrainStartMove()
    {
        OnTrainStartMove?.Invoke();
    }

    public event Action<TakeNeedArea> OnAddedNewNeed;
    public void AddedNewNeed(TakeNeedArea canTakeArea)
    {
        OnAddedNewNeed?.Invoke(canTakeArea);
    }

    public event Action<TakeNeedArea> OnDisableNeed;
    public void DisableNeed(TakeNeedArea canTakeArea)
    {
        OnDisableNeed?.Invoke(canTakeArea);
    }

    public event Action<CleanArea> OnAddedToClean;
    public void AddedToClean(CleanArea cleanArea)
    {
        OnAddedToClean?.Invoke(cleanArea);
    }

    public event Action<CleanArea> OnCleanArea;
    public void CleanArea(CleanArea cleanArea)
    {
        OnCleanArea?.Invoke(cleanArea);
    }
    public event Action<CanGiveArea> OnAddedCanGiveArea;
    public void AddedCanGiveArea(CanGiveArea canGiveArea)
    {
        OnAddedCanGiveArea?.Invoke(canGiveArea);
    }
    public event Action<int> OnBuyObject;
    public void BuyObject(int expValue)
    {
        OnBuyObject?.Invoke(expValue);
    }
}
