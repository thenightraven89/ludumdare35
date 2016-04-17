using System.Collections.Generic;
using UnityEngine;

public class SightBehaviour : MonoBehaviour
{
    public delegate void DetectedEvent(string name);
    public event DetectedEvent OnDetected = delegate { };

    private const float MAX_DISTANCE = 10f;

    [SerializeField]
    private string[] _detectable;

    [SerializeField]
    private GameObject _exclamationMark;

    private RaycastHit _closestInfo;
    private RaycastHit[] _hits;

    private Transform _t;
    private List<string> _detectableList;

    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Awake()
    {
        _t = transform;
        _detectableList = new List<string>(_detectable);
    }


    private void Update()
    {
        //Debug.DrawLine(_t.position, _target.position);

        var ray = new Ray(_t.position, (_target.position - _t.position).normalized);

        Debug.DrawRay(_t.position, (_target.position - _t.position).normalized);

        _hits = Physics.RaycastAll(ray, MAX_DISTANCE);

        if (_hits.Length > 0)
        {
            _closestInfo = _hits[0];

            // detect closest;
            foreach (var h in _hits)
            {
                if (Vector3.Distance(h.collider.transform.position, _t.position) <
                    Vector3.Distance(_closestInfo.collider.transform.position, _t.position))
                {
                    _closestInfo = h;
                }
            }

            //bool hit = Physics.Raycast(new Ray(_t.position, _t.forward),out _info, MAX_DISTANCE);

            var angle = Vector3.Angle(
                _t.forward,
                (_closestInfo.collider.transform.position - _t.position).normalized);

            if (_detectableList.Contains(_closestInfo.collider.name) && angle <= 90f)
            {
                _exclamationMark.SetActive(true);

                var vampire = _closestInfo.collider.GetComponentInParent<Vampire>();
                if (vampire != null)
                {
                    vampire.enabled = false;
                    vampire.StopAnimation();
                }

                var patrol = _t.GetComponent<PatrolBehaviour>();

                if (patrol != null)
                {
                    patrol.StopAnimation();
                    patrol.enabled = false;
                }

                this.enabled = false;

                _t.LookAt(vampire.transform);
                //_t.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                OnDetected(_closestInfo.collider.name);
            }
        }
    }
}