using UnityEngine;

public class RotateBehaviour : MonoBehaviour
{
    private const float SPEED = 100f;

    private Transform _t;

    private void Awake()
    {
        _t = transform;
    }

    private void Update()
    {
        _t.Rotate(Vector3.up, Time.deltaTime * SPEED, Space.Self);
    }
}