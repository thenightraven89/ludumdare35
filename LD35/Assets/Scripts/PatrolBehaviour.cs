using System;
using System.Collections;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform[] _waypoints;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _stationaryTime;

    [SerializeField]
    private Animator _animator;

    private Transform _t;
    private int _targetIndex;

    private bool _isResting;
    
    private void Awake()
    {
        _t = transform;

        if (_waypoints.Length > 2) return;

        _t.position = _waypoints[0].position;
        _t.rotation = _waypoints[0].rotation;
        _targetIndex = 0;
        _isResting = false;
    }

    private void Update()
    {
        if (_waypoints.Length < 2 || _isResting) return;

        if (_t.position != _waypoints[_targetIndex].position)
        {
            _t.position = Vector3.MoveTowards(
                _t.position,
                _waypoints[_targetIndex].position,
                _speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(SetNextWaypoint());
        }
    }

    private IEnumerator SetNextWaypoint()
    {
        _isResting = true;
        StopAnimation();

        yield return new WaitForSeconds(_stationaryTime);

        _targetIndex = (_targetIndex + 1) % _waypoints.Length;
        _t.LookAt(_waypoints[_targetIndex]);
        _isResting = false;
        StartAnimation();
    }

    internal void StopAnimation()
    {
        _animator.SetBool("Walk", false);
    }

    internal void StartAnimation()
    {
        _animator.SetBool("Walk", true);
    }
}