﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    private bool _isBusy = false;
    private const float MOVEMENT_SPEED = .03f;

    public delegate void BloodChangeEvent(int value);
    public event BloodChangeEvent OnBloodChange = delegate { };

    public delegate void SpeakEvent(string value);
    public event SpeakEvent OnSpeak = delegate { };

    public delegate void CollectedEvent(string name);
    public event CollectedEvent OnCollected = delegate { };

    public delegate void ConsumedEvent(string name);
    public event ConsumedEvent OnConsumed = delegate { };

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

    [SerializeField]
    private ParticleSystem _bloodPS;

    private Animator _vampireAnimator;

    private List<string> _items;

    private const int MAX_BLOOD = 7;

    public int Blood
    {
        get { return _blood; }

        set
        {
            _blood = Mathf.Clamp(value, 0, 7);
            OnBloodChange(_blood);
        }
    }

    private void Awake()
    {
        _items = new List<string>();
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
        var inputExit = Input.GetButtonDown("Escape");

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

        if (inputFeed && !_isBusy && _state == VampireState.Human)
        {
            RaycastHit info;
            var isObstacle = Physics.Raycast(
                new Ray(transform.position + transform.forward + Vector3.up, Vector3.down),
                out info);

            if (isObstacle)
            {
                var victim = info.collider.GetComponent<VictimBehaviour>();

                var virgin = info.collider.GetComponent<VirginBehaviour>();

                if (virgin != null)
                {
                    OnSpeak(virgin.Text);
                }

                if (victim != null && victim.transform.forward == transform.forward)
                {
                    StartCoroutine(FeedCoroutine(victim));   
                }
                else
                {
                    OnSpeak("Can't feed from here.");
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

        if (inputExit)
        {
            Application.Quit();
        }
    }

    private IEnumerator FeedCoroutine(VictimBehaviour victim)
    {
        _isBusy = true;

        _vampireAnimator.SetTrigger("Feed");

        yield return new WaitForSeconds(.5f);
        
        _bloodPS.Emit(10);
        Blood += victim.Life;
        Destroy(victim.gameObject);

        _isBusy = false;
    }
    

    private IEnumerator Move(Vector3 delta)
    {
        var destination = transform.position + delta;

        transform.LookAt(destination);

        RaycastHit info;
        var hit = Physics.Raycast(
            new Ray(destination + Vector3.up, Vector3.down),
            out info);

        if (hit && !info.collider.isTrigger)
        {
            var lockBehaviour = info.collider.gameObject.GetComponent<LockBehaviour>();
            if (lockBehaviour != null)
            {
                if (_items.Contains(lockBehaviour.Key))
                {
                    OnConsumed(lockBehaviour.Key);
                    _items.Remove(lockBehaviour.Key);
                    Destroy(info.collider.gameObject);
                }
            }
        }

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

    private void OnTriggerEnter(Collider other)
    {
        var collectable = other.GetComponent<CollectableBehaviour>();
        if (collectable != null)
        {
            var obj = other.gameObject;
            _items.Add(obj.name);
            OnCollected(obj.name);
            Destroy(obj);
        }
    }

    internal void StopAnimation()
    {
        var animators = GetComponentsInChildren<Animator>();
        foreach (var a in animators)
        {
<<<<<<< HEAD
            a.enabled = false;
=======
			a.enabled = false;
>>>>>>> 4a6f059e2e21424d3edd0ff148564bd87427e2cf
        }
    }

    public enum VampireState
    {
        Human,
        Bat,
        Worg
    }
}