using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JoystickHandler : MonoBehaviour
{
    private Transform _outerJoystick;
    private Transform _innerJoystick;

    [SerializeField] private float _lerpTime = .8f;
    [SerializeField] private float _touchSensitivity = 1f;


    float tapCount, _counter, _lerpRadius, _outerFinalRadius, _lerpInnerTransparency, _counterStart, _counterStartForClose, _innerFinalTransparency;
    private bool _inputValid, _touchWasOnJoystick, _snapBack, _doubleTap;


    private Material _mat_OuterJoystick;
    private Material _mat_InnerJoystick;


    [SerializeField] private float _distBetweenInnertoOuterJoystick = .5f;


    // Start is called before the first frame update
    void Start()
    {
        _outerJoystick = transform;
        _innerJoystick = GetComponentInChildren<Collider2D>().transform;

        UI_InputDetect_Joystick.ValidJoyStickTouchEvent.AddListener(ProcessJoystickInput);
        UI_InputDetect_Joystick.ValidDoubleTapEvent.AddListener(ProcessDoubleTap);
        UI_InputDetect_Joystick.JoystickTriggerSnapback.AddListener(ProcessSnapBack);

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
            radius = _outerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _innerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _distBetweenInnertoOuterJoystick;

            center = _outerJoystick.GetComponent<MeshRenderer>().bounds.center;
            center.z = _innerJoystick.position.z;

            pos = new Vector3(pos.x, pos.y, _innerJoystick.position.z);
            dir = pos - center;

            float angRad = Mathfs.GetAngleByUnitVector(dir.normalized);

            UI_InputDetect_Joystick.JoystickDirInDegrees = (angRad > 0 ? angRad : (2 * Mathfs.PI + angRad)) * 360 / (2 * Mathfs.PI); //Remap from  [ 0 - 180, -180 - 0 ] to [ 0 - 360 ]
            UI_InputDetect_Joystick.ValidJoystickInput = true;

            newPos = center + (dir.normalized * radius);



            _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
        }
      

        else if (touch.phase == TouchPhase.Ended)
        {
            if (_inputValid) _touchWasOnJoystick = true;
            _inputValid = false;
            _snapBack = true;

            Debug.Log("Snapping back");
        }
    }

    private void FixedUpdate()
    {
        //if (_doubleTap)
        //{
        //    Debug.Log("Double Tapping");
        //    // Start Fade In
        //    if (!UI_InputDetect_Joystick.JoystickStateClosed) StartCoroutine(CloseJoystick());
        //    else StartCoroutine(Fade());
        //}

        if (!_inputValid)
        {
            //   MoveInnerJoystick(touch);
            UI_InputDetect_Joystick.ValidJoystickInput = false;
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
        if (!UI_InputDetect_Joystick.JoystickStateClosed) StartCoroutine(CloseJoystick());
        else StartCoroutine(Fade());

        //if (touch == )
        //{

        //}

    }

    private void ProcessSnapBack()
    {
        _innerJoystick.position = Mathfs.LerpLinear(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z), Time.deltaTime * _touchSensitivity);
        if (Vector3.Distance(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z)) <= 0.01)
        {
            _innerJoystick.position = new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z);
            _snapBack = false;
        }
    }
    // Checking for Double Tap secondary after single touch in Update
    //void LateUpdate()
    //{
    //    // Register Valid Touches for Double Tap
    //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && _touchWasOnJoystick)
    //    {
    //        tapCount += 1;
    //        StartCoroutine(Countdown());
    //        _touchWasOnJoystick = false;
    //    }

    //    // If Valid Double Tap Fade In / Out
    //    if (tapCount == 2)
    //    {
    //        _doubleTap = true;
    //        if (UI_InputDetect_Joystick.JoystickStateClosed) _counter = _counterStart;
    //        else _counter = _counterStartForClose;

    //        tapCount = 0;
    //        StopCoroutine(Countdown());
    //    }

    //}

    //private IEnumerator Countdown()
    //{
    //    yield return new WaitForSeconds(_doubleTapSensitivity);
    //    tapCount = 0;
    //}

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
        ChangeJoystickState(false);

    }

    private IEnumerator CloseJoystick()
    {
        _counter -= Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _outerFinalRadius, _counter / _lerpTime);
        _mat_OuterJoystick.SetFloat("_Radius", _lerpRadius);

        // Fade in inner Transparency
        _lerpInnerTransparency = Mathf.Lerp(.8f, _innerFinalTransparency, _counter / _lerpTime);
        _mat_InnerJoystick.SetFloat("_Transparency", _lerpInnerTransparency);

        yield return new WaitUntil(() => _counter <= _counterStart);

        _doubleTap = false;
        ChangeJoystickState(true);

        UI_InputDetect_Joystick.ValidJoystickInput = false;

    }

    private void ChangeJoystickState(bool newState)
    {
        UI_InputDetect_Joystick.JoystickStateClosed = newState;
        UI_InputDetect_Joystick.JoystickStateChanged.Invoke();
    }
}
