using System.Collections;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    private bool _isBusy = false;
    private const float MOVEMENT_SPEED = .03f;

    public delegate void BloodChangeEvent(int value);
    public event BloodChangeEvent OnBloodChange = delegate { };

    public delegate void SpeakEvent(string value);
    public event SpeakEvent OnSpeak = delegate { };

    private VampireState _state;

    /// <summary>
    /// use Blood instead of this;
    /// </summary>
    private int _blood;

    [SerializeField]
    private GameObject _vampireBody;

    [SerializeField]
    private GameObject _batBody;

    [SerializeField]
    private ParticleSystem _ps;

    private Animator _vampireAnimator;

    public int Blood
    {
        get { return _blood; }

        set
        {
            _blood = Mathf.Max(0, value);
            OnBloodChange(value);
        }
    }

    private void Awake()
    {
        _vampireAnimator = _vampireBody.GetComponent<Animator>();
        SwitchState(VampireState.Human);
    }

    private void Update()
    {
        var inputMoveH = Input.GetAxis("Horizontal");
        var inputMoveV = Input.GetAxis("Vertical");
        var inputInteract = Input.GetButtonDown("Interact");
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

        if (inputInteract && !_isBusy)
        {
            RaycastHit info;
            var isObstacle = Physics.Raycast(
                new Ray(transform.position + transform.forward + Vector3.up, Vector3.down),
                out info);

            if (isObstacle)
            {
                var text = info.collider.GetComponent<InteractionText>();
                if (text != null)
                {
                    OnSpeak(text.GetRandom());
                }
            }
        }

        if (inputMoveH != 0 && !_isBusy)
        {
            StartCoroutine(Move(new Vector3(Mathf.Sign(inputMoveH), 0, 0)));
        }
        
        if (inputMoveV != 0 && !_isBusy)
        {
            StartCoroutine(Move(new Vector3(0, 0, Mathf.Sign(inputMoveV))));
        }

        if (inputMoveH == 0 &&
            inputMoveV == 0 &&
            _state == VampireState.Human &&
            _vampireBody.activeSelf)
        {
            if (_vampireAnimator.GetBool("Walk"))
            {
                _vampireAnimator.SetBool("Walk", false);
            }
        }
    }
    

    private IEnumerator Move(Vector3 delta)
    {
        var destination = transform.position + delta;

        transform.LookAt(destination);

        RaycastHit info;
        var hit = Physics.Raycast(
            new Ray(destination + Vector3.up, Vector3.down),
            out info);

        if ((!hit || hit && info.collider.isTrigger) &&
            (_state == VampireState.Bat && Blood > 0 || _state != VampireState.Bat))
        {
            _isBusy = true;

            if (_state == VampireState.Human && _vampireBody.activeSelf)
            {
                _vampireBody.GetComponent<Animator>().SetBool("Walk", true);
            }

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
        _ps.Emit(15);

        _state = newState;

        switch (newState)
        {
            case VampireState.Human:
                _vampireBody.SetActive(true);
                _batBody.SetActive(false);
                break;

            case VampireState.Bat:
                _vampireBody.SetActive(false);
                _batBody.SetActive(true);
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