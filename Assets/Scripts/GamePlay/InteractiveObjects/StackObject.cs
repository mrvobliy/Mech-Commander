using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackObject : MonoBehaviour
{
    public event Action<int> OnStackCountChanged;

    private const float OBJECTS_SPACING = 0.13f;

    public List<CarringObject> _objectStack = new List<CarringObject>();

    public int _maxObjectCount;
    public int _currentObjectCount;

    public int _objectCountX, _objectCountZ;
    public float _objectSizeX, _objectSizeY, _objectSizeZ;

    public List<CarringObject> ObjectStack => _objectStack;
    public void AddObject(CarringObject bufObjeñt)
    {
        _currentObjectCount++;

        bufObjeñt.transform.SetParent(transform);
        bufObjeñt.transform.DOLocalJump(GetStackPosition(), 1f, 1, 0.35f).SetAutoKill(true);
        _objectStack.Add(bufObjeñt);

        OnStackCountChanged?.Invoke(_objectStack.Count);
    }
    public bool IsHasEmptySlot()
    {
        return _currentObjectCount < _maxObjectCount;
    }
    private Vector3 GetStackPosition()
    {
        float xPosition = ((_currentObjectCount - 1) % _objectCountX) * (_objectSizeX + OBJECTS_SPACING);
        float zPosition = (((_currentObjectCount - 1) / _objectCountX) % _objectCountZ) * (_objectSizeZ + OBJECTS_SPACING);
        float yPosition = ((_currentObjectCount - 1) / (_objectCountX * _objectCountZ)) * (_objectSizeY + OBJECTS_SPACING);

        Vector3 targetPosition = new Vector3(xPosition, yPosition, -zPosition);

        return targetPosition;
    }
    private Vector3 GetStackPosition(int fishIndex)
    {
        float xPosition = ((fishIndex) % _objectCountX) * (_objectSizeX + OBJECTS_SPACING);
        float zPosition = (((fishIndex) / _objectCountX) % _objectCountZ) * (_objectSizeZ + OBJECTS_SPACING);
        float yPosition = ((fishIndex) / (_objectCountX * _objectCountZ)) * (_objectSizeY + OBJECTS_SPACING);

        Vector3 targetPosition = new Vector3(xPosition, yPosition, -zPosition);

        return targetPosition;
    }

    public void TransferObjectTo(CarringObject targetGameObject, StackObject targetStack)
    {
        targetStack.AddObject(targetGameObject);
        _objectStack.Remove(targetGameObject);
        _currentObjectCount = _objectStack.Count;
        ResetObjectPositions();

        OnStackCountChanged?.Invoke(_objectStack.Count);
    }
    public void TransferObjectTo(CarringObject targetGameObject, Transform toTargetObject)
    {
        _objectStack.Remove(targetGameObject);
        _currentObjectCount = _objectStack.Count;
        ResetObjectPositions();

        targetGameObject.transform.SetParent(toTargetObject);
        targetGameObject.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.35f).SetAutoKill(true).OnKill(() => Destroy(targetGameObject.gameObject));

        OnStackCountChanged?.Invoke(_objectStack.Count);
    }

    public void ResetObjectPositions()
    {
        for (int i = 0; i < _objectStack.Count; i++)
        {
            _objectStack[i].transform.localPosition = GetStackPosition(i);
        }
    }

    public void ClearStack()
    {
        if (_objectStack.Count > 0)
        {
            for (int i = 0; i < _objectStack.Count; i++)
            {
                Destroy(_objectStack[i].gameObject);
            }
            _objectStack.Clear();

            OnStackCountChanged?.Invoke(_objectStack.Count);
        }
    }
}
