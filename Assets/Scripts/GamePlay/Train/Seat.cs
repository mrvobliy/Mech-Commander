using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private List<CanGiveArea> _seatPlace = new List<CanGiveArea>();
    [SerializeField] private int _giveMoneyPerLevel;

    private BuyingObject _seatBuying;
    public List<CanGiveArea> SeatPlace => _seatPlace;

    private void Awake()
    {
        _seatBuying = GetComponent<BuyingObject>();
    }
    public bool IsGetFreeSeat(out CanGiveArea freeSeat)
    {
        freeSeat = null;
        for(int i = 0; i < _seatPlace.Count; i++)
        {
            if (!_seatPlace[i].IsReserved)
            {
                freeSeat = _seatPlace[i];
                _seatPlace[i].IsReserved = true;
                return true;
            }
        }
        return false;
    }

    public void OnTrainArrived()
    {
        for (int i = 0; i < _seatPlace.Count; i++)
        {
            if(_seatPlace[i].IsReserved == true)
            {
                _seatPlace[i].AddObjectNumber(1);
                _seatPlace[i].IsReserved = false;
            }
        }
    }

    public int GetMoneyForTicket()
    {
        return _giveMoneyPerLevel * (_seatBuying.CurrentIndexBuyedObject + 1);
    }
}
