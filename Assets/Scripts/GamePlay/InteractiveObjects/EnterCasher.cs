using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCasher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<AIMoveThrowAreas>(out AIMoveThrowAreas passenger))
        {
            MoneyPayer.Pay(passenger.CurrentSeatCost, other.transform.position, new Vector3(0f, transform.position.y, transform.position.z));
        }
    }
}
