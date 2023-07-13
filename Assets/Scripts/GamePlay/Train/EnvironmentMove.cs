using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMove : MonoBehaviour
{
    public Transform[] _pathPoints;
    public float _speed = 5f;

    private int _currentPoint = 1;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _pathPoints[_currentPoint].position, _speed * Time.deltaTime);

        if (transform.position.z <= _pathPoints[1].position.z)
        {
            OnGetEndPoint();
        }
    }

    public virtual void OnGetEndPoint()
    {
        transform.position = _pathPoints[0].position;
    }
}
