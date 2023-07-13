using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveFadeUp : MonoBehaviour
{
    public float _from = 0.5f, _duration;
    private void OnEnable()
    {
        transform.localPosition = Vector3.down * _from;
        transform.DOLocalMove(Vector3.zero, _duration).SetAutoKill(true);
    }
}
