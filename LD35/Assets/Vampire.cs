using System.Collections;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    private bool _isBusy = false;
    private const float MOVEMENT_SPEED = .05f;

    private void Update()
    {
        var inputMoveH = Input.GetAxis("Horizontal");
        var inputMoveV = Input.GetAxis("Vertical");
        var inputShapeshift = Input.GetButtonDown("Shapeshift");
        var inputFeed = Input.GetButtonDown("Feed");

        if (inputMoveH != 0 && !_isBusy)
        {
            StartCoroutine(Move(new Vector3(Mathf.Sign(inputMoveH), 0, 0)));
        }
        
        if (inputMoveV != 0 && !_isBusy)
        {
            StartCoroutine(Move(new Vector3(0, 0, Mathf.Sign(inputMoveV))));
        }

        if (inputShapeshift && !_isBusy)
        {

        }

        if (inputFeed && !_isBusy)
        {

        }
    }

    private IEnumerator Move(Vector3 delta)
    {
        var destination = transform.position + delta;

        transform.LookAt(destination);

        var isObstacle = Physics.Raycast(new Ray(destination + Vector3.up, Vector3.down));

        if (!isObstacle)
        {
            _isBusy = true;

            while (transform.position != destination)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    destination,
                    MOVEMENT_SPEED);

                yield return null;
            }

            _isBusy = false;
        }
    }
}