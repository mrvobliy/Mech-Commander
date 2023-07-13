using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedCanvas : MonoBehaviour
{
    [SerializeField] private Image _needImage;
    private NeedData _needData;
    private void Awake()
    {
       _needData = Resources.Load<NeedData>("NeedData");
    }
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(310f, 0f, 0f);
    }

    public void OnEnableNeed(CarringObjectType needType)
    {
        _needImage.sprite = _needData._needSprites[(int)needType];
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(310f, 0f, 0f);
    }
}
