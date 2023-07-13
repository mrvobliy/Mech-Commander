using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public static class MoneyPayer
{
    private static readonly GameObject moneyPrefab;

    static MoneyPayer()
    {
        moneyPrefab = Resources.Load<GameObject>("MoneyPicked");
    }

    public static void Pay(int cost, Vector3 fromPosition, Vector3 targetPosition)
    {
        if (moneyPrefab == null)
        {
            Debug.LogError("Money prefab is not set");
            return;
        }
        RandomSeed.Time();
        GameObject moneyObject = Object.Instantiate(moneyPrefab, fromPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));

        MoneyPicked moneyComponent = moneyObject.GetComponent<MoneyPicked>();

        moneyComponent.AddCost(cost);

        targetPosition = targetPosition + new Vector3(Random.Range(-0.9f, 0.9f), 0f, Random.Range(-0.9f, 0.9f));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 100f, NavMesh.AllAreas))
        {
            Vector3 targetPoint = hit.position;

            float jumpPower = 3f;
            int jumpCount = 1;
            float duration = Vector3.Distance(fromPosition, targetPoint) / 4f;
            moneyObject.transform.DOJump(targetPoint + Vector3.up * 0.05f, jumpPower, jumpCount, duration).SetEase(Ease.InSine);
        }
        else
        {
            Debug.LogError("Unable to find valid NavMesh point for target position");
            Object.Destroy(moneyObject);
        }
    }
}
