using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestroyOnCollision : MonoBehaviour
{
    private void Start()
    {
        transform.DOLocalMoveY(1, 0.35f).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnTriggerEnter(Collider other)
    {
        transform.DOKill();
        transform.DOScale(0f, 0.3f).OnComplete(() => OnEndTween()).SetAutoKill(true);
    }

    private void OnEndTween()
    {
        Destroy(gameObject);
    }
}
