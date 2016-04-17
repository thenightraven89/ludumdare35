using System;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform[] _waypoints;

    [SerializeField]
    private float _speed;

    private Transform _t;
    private int _targetIndex = 0;

    private void Awake()
    {
        _t = transform;

        if (_waypoints.Length > 2) return;

        _t.position = _waypoints[0].position;
        _t.rotation = _waypoints[0].rotation;

        SetNextWaypoint();
    }

    private void Update()
    {
        if (_waypoints.Length < 2) return;

        if (_t.position != _waypoints[_targetIndex].position)
        {
            _t.position = Vector3.MoveTowards(
                _t.position,
                _waypoints[_targetIndex].position,
                _speed * Time.deltaTime);
        }
        else
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        _targetIndex = (_targetIndex + 1) % _waypoints.Length;
        _t.LookAt(_waypoints[_targetIndex]);
    }

    internal void StopAnimation()
    {
        var animators = GetComponentsInChildren<Animator>();
        foreach (var a in animators)
        {
            a.Stop();
        }
    }
}