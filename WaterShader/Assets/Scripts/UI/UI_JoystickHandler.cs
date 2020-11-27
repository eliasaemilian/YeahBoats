using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_JoystickHandler : TappableGameobject
{
    public bool JoystickStateClosed { get; private set; } // [ false ] open, -> State: Moving, [ true ] closed -> State: Fishing
    public static float JoystickDirInDegrees { get; private set; }
    public static bool ValidJoystickInput { get; private set; } = false;

    public static UnityEvent JoystickStateChanged;
    public static UnityEvent BlockJoystickMovement;
    public static UnityEvent UnblockJoystickMovement;

    private bool _joystickBlocked;

    [SerializeField] private float _lerpTime = .8f;
    [SerializeField] private float _touchSensitivity = 1f;
    [SerializeField] private float _distBetweenInnertoOuterJoystick = .5f;

    [SerializeField] private AudioClip _joystickOpenSound = null;
    [SerializeField] private AudioClip _joystickCloseSound = null;

    private Vector3 _center;
    private float _counter, _lerpRadius, _outerFinalRadius, _lerpInnerTransparency, _counterStart, _counterStartForClose, _innerFinalTransparency;
    private bool _snapBack, _doubleTap;

    private Transform _outerJoystick;
    private Transform _innerJoystick;
    private Material _mat_OuterJoystick;
    private Material _mat_InnerJoystick;

    private float _radius;


    public override void OnStartInitialize()
    {
        _outerJoystick = transform;
        _innerJoystick = GetComponentInChildren<Collider2D>().transform;

        _radius = _outerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _innerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _distBetweenInnertoOuterJoystick; 


        UI_InputHandler.ValidTouchEvent2D.AddListener(OnTap);

        JoystickStateChanged = new UnityEvent();
        BlockJoystickMovement = new UnityEvent();
        UnblockJoystickMovement = new UnityEvent();
        BlockJoystickMovement.AddListener(BlockJoystick);
        UnblockJoystickMovement.AddListener(UnblockJoystick);

        _center = _outerJoystick.GetComponent<MeshRenderer>().bounds.center;
        _center.z = _innerJoystick.position.z;

        // Setup Shader Properties
        _mat_OuterJoystick = _outerJoystick.GetComponent<MeshRenderer>().material;
        _mat_InnerJoystick = _innerJoystick.GetComponent<MeshRenderer>().material;
        _outerFinalRadius = _mat_OuterJoystick.GetFloat("_Radius");
        _innerFinalTransparency = _mat_InnerJoystick.GetFloat("_Transparency");
        _counterStart = Mathfs.Remap(_outerFinalRadius * .33f, 0, _outerFinalRadius, 0, _lerpTime); // for Fade
        _counterStartForClose = Mathfs.Remap(_outerFinalRadius, 0, _outerFinalRadius, 0, _lerpTime); // for Closing
    }


    Vector3 dir, newPos;
    /// <summary>
    /// Calculate Joystick movement according to tap position.
    /// Calculates Degree Offset for Movement needed for the Boat Input
    /// </summary>
    /// <param name="touch"></param>
    /// <param name="pos"></param>
    public override void OnTap (Touch touch, Vector3 pos)
    {
        if (_joystickBlocked) return;

        base.OnTap(touch, pos);
        ValidJoystickInput = true;

        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
        {
            if (JoystickStateClosed) return;

            pos = new Vector3(pos.x, pos.y, _innerJoystick.position.z);
            dir = pos - _center;

            float angRad = Mathfs.GetAngleByUnitVector(dir.normalized);

            JoystickDirInDegrees = (angRad > 0 ? angRad : (2 * Mathfs.PI + angRad)) * 360 / (2 * Mathfs.PI); //Remap from  [ 0 - 180, -180 - 0 ] to [ 0 - 360 ]

            newPos = _center + (dir.normalized * _radius);

            _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
        }
      
    }


    public override void OnTapWasLetGo()
    {
        base.OnTapWasLetGo();

        _snapBack = true;
        ValidJoystickInput = false;

    }

    /// <summary>
    /// On Double Tap Joystick switches between Closed / Open
    /// Changing the Boat View between Fishing and Moving
    /// </summary>
    public override void OnDoubleTap()
    {
        base.OnDoubleTap();
      //  Debug.Log("Joystick is double tapped");
        if (_joystickBlocked) return;


        _doubleTap = true;

        if (JoystickStateClosed)
        {
            _counter = _counterStart;
            SoundscapeManager.PlaySoundWithClip.Invoke(_joystickOpenSound);
        }
        else
        {
            _counter = _counterStartForClose;
            SoundscapeManager.PlaySoundWithClip.Invoke(_joystickCloseSound);
        }
    }

    private void FixedUpdate()
    {
        if (_doubleTap)
        {
            // Start Fade In / Out
            if (!JoystickStateClosed) StartCoroutine(CloseJoystick());
            else StartCoroutine(Fade());
        }


        if (_snapBack)
        {
            // Lerp Joystick back to default position on tap let go
            _innerJoystick.position = Mathfs.LerpLinear(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z), Time.deltaTime * _touchSensitivity);
            if (Vector3.Distance(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z)) <= 0.01)
            {
                _innerJoystick.position = new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z);
                _snapBack = false;
            }
        }

    }


    public void ChangeJoystickState(bool newState)
    {
        if (newState == JoystickStateClosed) return;

        JoystickStateClosed = newState;
        JoystickStateChanged.Invoke();
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
        ChangeJoystickState(false);

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
        ChangeJoystickState(true);

        ValidJoystickInput = false;

    }


    private  void BlockJoystick() => _joystickBlocked = true;
    private  void UnblockJoystick() => _joystickBlocked = false;

}
