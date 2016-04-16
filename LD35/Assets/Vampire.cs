using System.Collections;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    private bool _isBusy = false;
    private const float MOVEMENT_SPEED = .025f;

    public delegate void BloodChangeEvent(int value);
    public event BloodChangeEvent OnBloodChange = delegate { };

    private VampireState _state;

    /// <summary>
    /// use Blood instead of this;
    /// </summary>
    private int _blood;

    public int Blood
    {
        get { return _blood; }

        set
        {
            _blood = Mathf.Max(0, value);
            OnBloodChange(value);
        }
    }

    private void Update()
    {
        var inputMoveH = Input.GetAxis("Horizontal");
        var inputMoveV = Input.GetAxis("Vertical");
        var inputBat = Input.GetButtonDown("Bat");
        var inputFeed = Input.GetButtonDown("Feed");

        if (inputBat && !_isBusy)
        {
            if (_state == VampireState.Bat)
            {
                SwitchState(VampireState.Human);
            }
            else if (_state == VampireState.Human && Blood > 0)
            {
                SwitchState(VampireState.Bat);
            } 
        }

        if (inputFeed && !_isBusy)
        {

        }

        if (inputMoveH != 0 && !_isBusy)
        {
            StartCoroutine(Move(new Vector3(Mathf.Sign(inputMoveH), 0, 0)));
        }
        
        if (inputMoveV != 0 && !_isBusy)
        {
            StartCoroutine(Move(new Vector3(0, 0, Mathf.Sign(inputMoveV))));
        }
    }
    

    private IEnumerator Move(Vector3 delta)
    {
        var destination = transform.position + delta;

        transform.LookAt(destination);

        var isObstacle = Physics.Raycast(new Ray(destination + Vector3.up, Vector3.down));

        if (!isObstacle &&
            (_state == VampireState.Bat && Blood > 0 || _state != VampireState.Bat))
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

            if (_state == VampireState.Bat)
            {
                Blood--;

                if (Blood == 0)
                {
                    SwitchState(VampireState.Human);
                }
            }

            _isBusy = false;
        }
    }

    private void SwitchState(VampireState newState)
    {
        _state = newState;

        switch (newState)
        {
            case VampireState.Human:
                GetComponent<Renderer>().enabled = true;
                break;

            case VampireState.Bat:
                GetComponent<Renderer>().enabled = false;
                break;

            case VampireState.Worg:
                break;
        }
    }

    public enum VampireState
    {
        Human,
        Bat,
        Worg
    }
}