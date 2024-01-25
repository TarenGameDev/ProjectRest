using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper
{
    Transform _movingObject;
    Vector3 _startPosition, _endPosition;
    float _speed, _totalDistance;
    bool _run;

    public Lerper(Transform obj, Vector3 end, float speed, bool run)
    {
        _movingObject = obj;
        _startPosition = _movingObject.position;
        _endPosition = end;
        _speed = speed;
        _totalDistance = (_endPosition - _startPosition).magnitude;
        _run = run;
    }

    public void Tick()
    {
        if (!_run) return;

        float distanceFromStart = (_movingObject.position - _startPosition).magnitude;
        distanceFromStart += Time.deltaTime * _speed;
        float progress = distanceFromStart / _totalDistance;

        _movingObject.position = Vector3.Lerp(_startPosition, _endPosition, progress);

        if (progress >= 0.95f) _run = false;


    }
}
