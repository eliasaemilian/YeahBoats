using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UI_InputDetect_Joystick : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera = null;
    [SerializeField] private Transform _outerJoystick = null;
    [SerializeField] private Transform _innerJoystick = null;
    [SerializeField] private float _touchSensitivity = 1f;
    [SerializeField] private float _doubleTapSensitivity = .3f;
    [SerializeField] private float _lerpTime = .8f;

    private bool _inputValid, _touchWasOnJoystick, _snapBack, _doubleTap, _joystickClosed;

    float tapCount, _counter, _lerpRadius, _outerFinalRadius, _lerpInnerTransparency, _counterStart, _counterStartForClose, _innerFinalTransparency;

    Plane objPlane;

    private Material _mat_OuterJoystick;
    private Material _mat_InnerJoystick;

// Start is called before the first frame update
void Start()
    {
        // Check for UI Camera avaliable
        if (_uiCamera == null)
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            for (int i = 0; i < cameras.Length; i++)
            {
                var cameraData = cameras[i].GetUniversalAdditionalCameraData();
                if (cameraData.renderType == CameraRenderType.Overlay) _uiCamera = cameras[i];
            }
            Debug.LogWarning("No UI Camera selected for Input System"); // [!] This will break in a scene with more than 1 overlay camera
        }

        // Setup Plane for Touch Input Checks
        objPlane = new Plane(_uiCamera.transform.forward * -1, _outerJoystick.position);

        // Setup Shader Properties
        _mat_OuterJoystick = _outerJoystick.GetComponent<MeshRenderer>().material;
        _mat_InnerJoystick = _innerJoystick.GetComponent<MeshRenderer>().material;
        _outerFinalRadius = _mat_OuterJoystick.GetFloat("_Radius");
        _innerFinalTransparency = _mat_InnerJoystick.GetFloat("_Transparency");
        _counterStart = Mathfs.Remap(_outerFinalRadius * .33f, 0, _outerFinalRadius, 0, _lerpTime); // for Fade
        _counterStartForClose = Mathfs.Remap(_outerFinalRadius, 0, _outerFinalRadius, 0, _lerpTime); // for Closing
    }

    private Touch _touch;
    void Update()
    {

        if (Input.touchCount > 0 )
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began || _touch.phase == TouchPhase.Moved)
            {
                ProcessTouchOnJoystick(_touch);

            }
                  

            else if (_touch.phase == TouchPhase.Ended)
            {
                if (_inputValid) _touchWasOnJoystick = true;
                _inputValid = false;
                _snapBack = true;
            }
        }


    }

    private void FixedUpdate()
    {
        if (_doubleTap)
        {
            Debug.Log("Double Tapping");
            // Start Fade In
            if (!_joystickClosed) StartCoroutine(CloseJoystick());
            else StartCoroutine(Fade());
        }

        if (_inputValid)
        {
         //   MoveInnerJoystick(touch);

        }

        if (_snapBack)
        {
            _innerJoystick.position = Mathfs.LerpLinear(_innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z), Time.deltaTime * _touchSensitivity);
            if ( Vector3.Distance( _innerJoystick.position, new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z) ) <= 0.01 )
            {
                _innerJoystick.position = new Vector3(_outerJoystick.position.x, _outerJoystick.position.y, _innerJoystick.position.z);
                _snapBack = false;
            }
        }
    }

    // Checking for Double Tap secondary after single touch in Update
    void LateUpdate()
    {
        // Register Valid Touches for Double Tap
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && _touchWasOnJoystick)
        {
            tapCount += 1;
            StartCoroutine(Countdown());
            _touchWasOnJoystick = false;
        }

        // If Valid Double Tap Fade In / Out
        if (tapCount == 2)
        {
            _doubleTap = true;
            if (_joystickClosed) _counter = _counterStart;
            else _counter = _counterStartForClose;

            tapCount = 0;
            StopCoroutine(Countdown());
        }

    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(_doubleTapSensitivity);
        tapCount = 0;
    }

    private IEnumerator Fade()
    {
        _counter += Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _outerFinalRadius, _counter / _lerpTime);
        _mat_OuterJoystick.SetFloat("_Radius", _lerpRadius);

        // Fade in inner Transparency
        _lerpInnerTransparency = Mathf.Lerp( .8f, _innerFinalTransparency, _counter / _lerpTime);
        _mat_InnerJoystick.SetFloat("_Transparency", _lerpInnerTransparency);

        yield return new WaitUntil(() => _counter >= _lerpTime);

        _doubleTap = false;
        _joystickClosed = false;
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
        _joystickClosed = true;

    }

    Vector3 startPos, center, dir, newPos;
    float radius;
    private void ProcessTouchOnJoystick(Touch touch)
    {
        //transform the touch position into word space from screen space
        Ray mRay = _uiCamera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, _innerJoystick.position.z));

        radius = _outerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _innerJoystick.GetComponent<MeshRenderer>().bounds.extents.x;

        center = _outerJoystick.GetComponent<MeshRenderer>().bounds.center;
        center.z = _innerJoystick.position.z;

        if (objPlane.Raycast(mRay, out float rayDistance))
        {
            _inputValid = true;

            if (!_joystickClosed)
            {
                startPos = mRay.GetPoint(rayDistance);

                dir = startPos - center;
                newPos = center + (dir.normalized * radius);

                _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
            }


        }
    }

}
