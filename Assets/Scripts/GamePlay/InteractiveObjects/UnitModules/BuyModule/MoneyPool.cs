using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyPool : MonoBehaviour
{
    public GameObject _moneyPrefab;
    public int _poolSize = 10;
    public float _speed = 5f;

    private Queue<GameObject> _moneyPool;

    private void Start()
    {
        _moneyPool = new Queue<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject money = Instantiate(_moneyPrefab, transform);
            money.SetActive(false);
            _moneyPool.Enqueue(money);
        }
    }

    public void SpawnMoney(Transform target)
    {
        if (_moneyPool.Count > 0)
        {
            GameObject money = _moneyPool.Dequeue();
            money.transform.position = transform.position;
            money.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            money.SetActive(true);
            MoveMoney(money.transform, target);
        }
    }

    private void MoveMoney(Transform moneyTransform, Transform target)
    {
        float distance = Vector3.Distance(moneyTransform.position, target.position);
        float duration = distance / _speed;
        moneyTransform.DOJump(target.position, 0.2f, 1, duration)
            .OnComplete(() =>
            {
                moneyTransform.gameObject.SetActive(false);
                _moneyPool.Enqueue(moneyTransform.gameObject);
            }).SetAutoKill(true);
    }
}
