using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[Serializable] public class ValidPlaneTouchEvent : UnityEvent<Touch, Vector3> { }
[Serializable] public class ValidTouchEvent : UnityEvent<Touch> { }

public class UI_InputHandler : MonoBehaviour
{
    public static ValidPlaneTouchEvent ValidJoyStickTouchEvent;
    public static ValidPlaneTouchEvent ValidWaterTouchEvent;
    public static ValidTouchEvent ValidDoubleTapEvent;
    public static bool JoystickStateClosed { get; set; } // [ false ] open, -> State: Moving, [ true ] closed -> State: Fishing
    public static float JoystickDirInDegrees { get; set; }
    public static bool ValidJoystickInput { get; set; } = false;

    public static UnityEvent JoystickStateChanged;
    public static UnityEvent DoubleTapProcessed;

    [SerializeField] private Camera _uiCamera = null;
    [SerializeField] private Transform _outerJoystick = null;
    [SerializeField] private Transform _innerJoystick = null;
    [SerializeField] private float _touchSensitivity = 1f;
    [SerializeField] private float _doubleTapSensitivity = .3f;
    [SerializeField] private float _lerpTime = .8f;
    [SerializeField] private float _distBetweenInnertoOuterJoystick = .5f;


    float tapCount;

    private Plane _uiPlane;
    private Plane _environmentPlane;

    private Transform _pointOfInterest; // Currently tapped with first registered touch

    [SerializeField] private Transform _waterPlane; //FOR TAP DEBUG


    // Start is called before the first frame update
    void Awake()
    {
        // SETUP INPUT EVENTS
        if (ValidJoyStickTouchEvent == null) ValidJoyStickTouchEvent = new ValidPlaneTouchEvent();
        if (ValidWaterTouchEvent == null) ValidWaterTouchEvent = new ValidPlaneTouchEvent();
        if (ValidDoubleTapEvent == null) ValidDoubleTapEvent = new ValidTouchEvent();

        JoystickStateChanged = new UnityEvent();
        DoubleTapProcessed = new UnityEvent();

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
        _uiPlane = new Plane(_uiCamera.transform.forward * -1, _outerJoystick.position);
        _environmentPlane = new Plane(_waterPlane.up, _waterPlane.position);

    }

    private Touch _touch;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            Vector3 rayPos, rayPos2D;
            // Check for Touches on Joystick
            if (CheckForHitOnPlane2D(_touch, _uiPlane, _innerJoystick, out rayPos2D))
            {
                SetPOI(_innerJoystick);
                ValidJoystickInput = true;
            }
            else if (CheckForHitOnPlane(_touch, _environmentPlane, _waterPlane, out rayPos))
            {
                if(JoystickStateClosed) ValidWaterTouchEvent.Invoke(_touch, rayPos);
            }
            else
            {
                SetPOI(null);
            }

            ValidJoyStickTouchEvent.Invoke(_touch, rayPos2D); //currently fires for all touches for ease of navigating, might change later idk

        }
        else ValidJoystickInput = false;

    }


    // Checking for Double Tap secondary after single touch in Update
    void LateUpdate()
    {
       
        // Register Valid Touches for Double Tap
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && _pointOfInterest != null) //<- needs to count what has been tapped
        {
            tapCount += 1;
            StartCoroutine(Countdown());
        }

        // If Valid Double Tap Fade In / Out
        if (tapCount == 2)
        {
            ValidDoubleTapEvent.Invoke(_touch);

            tapCount = 0;
            StopCoroutine(Countdown());
        }

    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(_doubleTapSensitivity);
        tapCount = 0;
    }

    private void SetPOI(Transform newPOI)
    {
        if (_pointOfInterest != newPOI)
        {
            tapCount = 0;
            _pointOfInterest = newPOI;
        }
    }



    public static void ChangeJoystickState(bool newState)
    {
        if (newState == JoystickStateClosed) return;

        JoystickStateClosed = newState;
        JoystickStateChanged.Invoke();
    }


    private bool CheckForHitOnPlane2D(Touch touch, Plane plane, Transform zValue, out Vector3 rayPos)
    {
        //transform the touch position into word space from screen space
        Ray mRay = _uiCamera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, zValue.position.z));
        rayPos = Vector3.zero;

        Vector3 touchPosWorld = _uiCamera.ScreenToWorldPoint(touch.position);
        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        // this will always get the touch pos in relation to the relevant plane
        if (plane.Raycast(mRay, out float rayDistance))
        {
            // if Joystick is activated, move accordingly, record Input for Boat
            if (!JoystickStateClosed)
            {
                rayPos = mRay.GetPoint(rayDistance);
                rayPos = new Vector3(rayPos.x, rayPos.y, zValue.position.z);
            }

        }

        if (hit.collider != null) return true;
        else return false;

    }

    private bool CheckForHitOnPlane(Touch touch, Plane plane, Transform zValue, out Vector3 rayPos)
    {
        //transform the touch position into word space from screen space
        Ray mRay = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, zValue.position.z));
        rayPos = Vector3.zero;
        
        // this will always get the touch pos in relation to the relevant plane
        if (plane.Raycast(mRay, out float rayDistance))
        {
            // if Joystick is activated, move accordingly, record Input for Boat
            if (!JoystickStateClosed)
            {
                rayPos = mRay.GetPoint(rayDistance);
                rayPos = new Vector3(rayPos.x, rayPos.y, zValue.position.z);
            }

        }

        if (Physics.Raycast(mRay, out RaycastHit h, 1000f))
        {
            if (h.collider.gameObject.transform == zValue) return true;
            else return false;
        }
        else return false;
    }

    public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint)
    {
        float length;
        float dotNumerator;
        float dotDenominator;
        Vector3 vector;
        intersection = Vector3.zero;

        //calculate the distance between the linePoint and the line-plane intersection point
        dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
        dotDenominator = Vector3.Dot(lineVec, planeNormal);

        if (dotDenominator != 0.0f)
        {
            length = dotNumerator / dotDenominator;

            vector = lineVec.normalized  * length;

            intersection = linePoint + vector;

            return true;
        }

        else
            return false;
    }

}
