using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarringObject : MonoBehaviour
{
    [SerializeField] private CarringObjectType _carringObjectType;
    [SerializeField] private bool _isAbstract;
    [SerializeField] private int _cost = 2;

    public int Cost => _cost;
    public CarringObjectType CarringObjectType => _carringObjectType;
    public bool IsAbstract => _isAbstract;
}
