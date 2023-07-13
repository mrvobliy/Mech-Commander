using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainShake : MonoBehaviour
{
    public float _rotationSpeed = 10f;

    private float _rotationZ = 0f;
    private float _rotationSign = 1;

    void Update()
    {
        
        _rotationZ += Time.deltaTime * _rotationSpeed * _rotationSign;
        if (_rotationZ >= 1f || _rotationZ <= -1f)
        {
            _rotationZ = _rotationSign;
            _rotationSign = -_rotationSign;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ);
    }
}
