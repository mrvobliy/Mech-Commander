using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private Door _door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Unit>(out Unit unit))
        {
            _door.Open();
            gameObject.SetActive(false);
        }
    }
}
