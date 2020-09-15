﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JoystickHandler : MonoBehaviour
{
    private Transform _outerJoystick;
    private Transform _innerJoystick;

    [SerializeField] private float _lerpTime = .8f;
    [SerializeField] private float _touchSensitivity = 1f;


    float _counter, _lerpRadius, _outerFinalRadius, _lerpInnerTransparency, _counterStart, _counterStartForClose, _innerFinalTransparency;
    private bool _snapBack, _doubleTap;


    private Material _mat_OuterJoystick;
    private Material _mat_InnerJoystick;


    [SerializeField] private float _distBetweenInnertoOuterJoystick = .5f;


    // Start is called before the first frame update
    void Start()
    {
        _outerJoystick = transform;
        _innerJoystick = GetComponentInChildren<Collider2D>().transform;

        UI_InputHandler.ValidJoyStickTouchEvent.AddListener(ProcessJoystickInput);
        UI_InputHandler.ValidDoubleTapEvent.AddListener(ProcessDoubleTap);

        // Setup Shader Properties
        _mat_OuterJoystick = _outerJoystick.GetComponent<MeshRenderer>().material;
        _mat_InnerJoystick = _innerJoystick.GetComponent<MeshRenderer>().material;
        _outerFinalRadius = _mat_OuterJoystick.GetFloat("_Radius");
        _innerFinalTransparency = _mat_InnerJoystick.GetFloat("_Transparency");
        _counterStart = Mathfs.Remap(_outerFinalRadius * .33f, 0, _outerFinalRadius, 0, _lerpTime); // for Fade
        _counterStartForClose = Mathfs.Remap(_outerFinalRadius, 0, _outerFinalRadius, 0, _lerpTime); // for Closing
    }

    // Update is called once per frame
    void Update()
    {

    }

    float radius;
    Vector3 center, dir, newPos;
    private void ProcessJoystickInput (Touch touch, Vector3 pos)
    {
        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
        {
            if (UI_InputHandler.JoystickStateClosed) return;

            radius = _outerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _innerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _distBetweenInnertoOuterJoystick;  //REFACTOR: SAVE ON START

            center = _outerJoystick.GetComponent<MeshRenderer>().bounds.center; //REFACTOR: SAVE ON START
            center.z = _innerJoystick.position.z; //REFACTOR: SAVE ON START

            pos = new Vector3(pos.x, pos.y, _innerJoystick.position.z);
            dir = pos - center;

            float angRad = Mathfs.GetAngleByUnitVector(dir.normalized);

            UI_InputHandler.JoystickDirInDegrees = (angRad > 0 ? angRad : (2 * Mathfs.PI + angRad)) * 360 / (2 * Mathfs.PI); //Remap from  [ 0 - 180, -180 - 0 ] to [ 0 - 360 ]

            newPos = center + (dir.normalized * radius);

            _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
        }
      

        else if (touch.phase == TouchPhase.Ended)
        {
            _snapBack = true;
        }
    }

    private void FixedUpdate()
    {
        if (_doubleTap)
        {
            Debug.Log("Double Tapping");

            // Start Fade In / Out
            if (!UI_InputHandler.JoystickStateClosed) StartCoroutine(CloseJoystick());
            else StartCoroutine(Fade());
        }


        if (_snapBack)
        {
            _innerJoystick.position = Mathfs.LerpLinear(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z), Time.deltaTime * _touchSensitivity);
            if (Vector3.Distance(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z)) <= 0.01)
            {
                _innerJoystick.position = new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z);
                _snapBack = false;
            }
        }

    }

    private void ProcessDoubleTap(Touch touch)
    {
        _doubleTap = true;

        if (UI_InputHandler.JoystickStateClosed) _counter = _counterStart;
        else _counter = _counterStartForClose;

    }
  

    private IEnumerator Fade()
    {
        _counter += Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _outerFinalRadius, _counter / _lerpTime);
        _mat_OuterJoystick.SetFloat("_Radius", _lerpRadius);

        // Fade in inner Transparency
        _lerpInnerTransparency = Mathf.Lerp(.8f, _innerFinalTransparency, _counter / _lerpTime);
        _mat_InnerJoystick.SetFloat("_Transparency", _lerpInnerTransparency);

        yield return new WaitUntil(() => _counter >= _lerpTime);

        _doubleTap = false;
        UI_InputHandler.ChangeJoystickState(false);

    }

    private IEnumerator CloseJoystick()
    {
        _counter -= Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _outerFinalRadius, _counter / _lerpTime);
        _mat_OuterJoystick.SetFloat("_Radius", _lerpRadius);

        // Fade out inner Transparency
        _lerpInnerTransparency = Mathf.Lerp(.8f, _innerFinalTransparency, _counter / _lerpTime);
        _mat_InnerJoystick.SetFloat("_Transparency", _lerpInnerTransparency);

        yield return new WaitUntil(() => _counter <= _counterStart);

        _doubleTap = false;
        UI_InputHandler.ChangeJoystickState(true);

        UI_InputHandler.ValidJoystickInput = false;

    }


}
