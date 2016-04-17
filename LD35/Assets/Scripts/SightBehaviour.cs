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

    private RaycastHit _info;
    private Transform _t;
    private List<string> _detectableList;

    private void Awake()
    {
        _t = transform;
        _detectableList = new List<string>(_detectable);
    }


    private void Update()
    {
        bool hit = Physics.Raycast(new Ray(_t.position, _t.forward),out _info, MAX_DISTANCE);
        if (hit)
        {
            if (_detectableList.Contains(_info.collider.name))
            {
                _exclamationMark.SetActive(true);

                var vampire = _info.collider.GetComponentInParent<Vampire>();
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

                _t.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
                OnDetected(_info.collider.name);
            }
        }
    }
}