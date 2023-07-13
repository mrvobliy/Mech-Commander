using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithDelay : MonoBehaviour
{
    [SerializeField] private float _delay;

    private void OnEnable()
    {
        StartCoroutine(SetDisableWithDelay(_delay));
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator SetDisableWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}
