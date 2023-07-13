using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyPicked : MonoBehaviour
{
    private InteractiveType _interactiveType;
    private int _cost;
    private Collider _collider;
    public int Cost => _cost;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _interactiveType = InteractiveType.Buying;
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Unit>(out Unit unit) && unit.InteractiveListType.Contains(_interactiveType))
        {
            _collider.enabled = false;
            Vector3 moveDirection = (other.transform.position - transform.position).normalized + Vector3.up;
            transform.DOKill();
            transform.DOMove(other.transform.position + moveDirection, 0.35f).OnComplete(() => OnFirstTweenComplete(other.transform));
            unit.UnitModuleList[unit.InteractiveListType.IndexOf(_interactiveType)].ActionInInterract(_cost);
        }
    }

    private void OnFirstTweenComplete(Transform targetParent)
    {
        transform.SetParent(targetParent);
        transform.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() => Object.Destroy(gameObject));
    }

    public void AddCost(int value)
    {
        _cost = value;
    }
}
