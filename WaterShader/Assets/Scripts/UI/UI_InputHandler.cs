using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[Serializable] public class ValidPlaneTouchEvent2D : UnityEvent<Touch, Vector3> { }
[Serializable] public class ValidPlaneTouchEvent : UnityEvent<Touch, Vector3, float> { }
[Serializable] public class ValidTouchEvent : UnityEvent<Touch> { }

public class UI_InputHandler : MonoBehaviour
{
    public static ValidPlaneTouchEvent2D ValidTouchEvent2D;
   // public static ValidPlaneTouchEvent ValidWaterTouchEvent;
    public static ValidTouchEvent ValidDoubleTapEvent;


    public static UnityEvent DoubleTapProcessed;

    public Camera UICamera = null;
    [SerializeField] private Transform _outerJoystick = null;
    [SerializeField] private Transform _innerJoystick = null;
//    [SerializeField] private float _touchSensitivity = 1f;
    [SerializeField] private float _doubleTapSensitivity = .3f;
    [SerializeField] private float _lerpTime = .8f;
    [SerializeField] private float _distBetweenInnertoOuterJoystick = .5f;


    float tapCount;

    private Plane _uiPlane;

    private Transform _pointOfInterest; // Currently tapped with first registered touch
    private Transform _lastPOI; // last registered touch

    [SerializeField] private Transform _waterPlane; //FOR TAP DEBUG
    [SerializeField] private Transform _tapEffectPlane; //FOR TAP DEBUG

    [SerializeField] private List<TappableGameobject> _tappableGameobjectsInScene;
    private TappableGameobject _tappableFocus;

    // Start is called before the first frame update
    void Awake()
    {
        // SETUP INPUT EVENTS
        if (ValidTouchEvent2D == null) ValidTouchEvent2D = new ValidPlaneTouchEvent2D();
   //     if (ValidWaterTouchEvent == null) ValidWaterTouchEvent = new ValidPlaneTouchEvent();
        if (ValidDoubleTapEvent == null) ValidDoubleTapEvent = new ValidTouchEvent();

        // New Fetch Events Implementation
        _tappableGameobjectsInScene = new List<TappableGameobject>();
        TappableGameobject[] tappablesInScene = FindObjectsOfType<TappableGameobject>();
        for (int i = 0; i < tappablesInScene.Length; i++)
        {
            tappablesInScene[i].OnInitialize();
            _tappableGameobjectsInScene.Add(tappablesInScene[i]);
        }



        DoubleTapProcessed = new UnityEvent();

        // Check for UI Camera avaliable
        if (UICamera == null)
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            for (int i = 0; i < cameras.Length; i++)
            {
                var cameraData = cameras[i].GetUniversalAdditionalCameraData();
                if (cameraData.renderType == CameraRenderType.Overlay) UICamera = cameras[i];
            }
            Debug.LogWarning("No UI Camera selected for Input System"); // [!] This will break in a scene with more than 1 overlay camera
        }

        // Setup Plane for Touch Input Checks
        _uiPlane = new Plane(UICamera.transform.forward * -1, _outerJoystick.position);

    }

    private Touch _touch;
    void Update()
    {
        CheckForTapsOnGameObjects();
        return;
        //if (Input.touchCount > 0)
        //{
            
        //    _touch = Input.GetTouch(0);
        //    Vector3 hitPos, rayPos2D;
        //    // Check for Touches on Joystick
        //    if (CheckForHitOnPlane2D(_touch, _uiPlane, _innerJoystick, out rayPos2D, out GameObject hitGO))
        //    {
        //        SetPOI(_innerJoystick);
        //        ValidJoystickInput = true;
        //    }
        //    else if (CheckForHitOnPlane(_touch, _uiPlane, _tapEffectPlane, _waterPlane, out hitPos, out GameObject hitGO3D,  out float dist))
        //    {
        //        SetPOI(_waterPlane);
        //      //  if(JoystickStateClosed) ValidWaterTouchEvent.Invoke(_touch, hitPos, dist);
        //        //    ValidWaterTouchEvent.Invoke(_touch, hitPos, dist);
        //    }
        //    else
        //    {
        //        SetPOI(null);
        //    }

        //    ValidTouchEvent2D.Invoke(_touch, rayPos2D); //currently fires for all touches for ease of navigating, might change later idk

        //}
        //else ValidJoystickInput = false;

    }

    Vector3 hitPos, rayPos2D;
    GameObject hitGO;
    private void CheckForTapsOnGameObjects()
    {
        if (Input.touchCount > 0)
        {

            _touch = Input.GetTouch(0);

            // 2D

            if (CheckForHitOnPlane2D(_touch, _uiPlane, _innerJoystick, out rayPos2D, out hitGO))
            {
                for (int i = 0; i < _tappableGameobjectsInScene.Count; i++)
                {
                    // Check for Taps on GOs with 2D Colliders
                    if (_tappableGameobjectsInScene[i].GOTapRef == hitGO)
                    {
                        _tappableGameobjectsInScene[i].OnTap(_touch, rayPos2D); // pass all here that was passed in event

                        SetPOI(_tappableGameobjectsInScene[i].GOTapRef.transform);
                        _tappableFocus = _tappableGameobjectsInScene[i];
                    }
                }

                //   SetPOI(hitGO.transform);
                if (hitGO.transform == _innerJoystick) UI_JoystickHandler.ValidJoystickInput = true; //TODO: Joystick Logic should be moved to Joysticks OnTap
            }

            // 3D 

            else if (CheckForHitOnPlane(_touch, _uiPlane, _tapEffectPlane, _waterPlane, out hitPos, out hitGO, out float dist))
            {
                for (int i = 0; i < _tappableGameobjectsInScene.Count; i++)
                {
                    // Check for Taps on GOs with 3D Colliders
                    if (_tappableGameobjectsInScene[i].GOTapRef == hitGO)
                    {
                        _tappableGameobjectsInScene[i].OnTap(_touch, hitPos, dist); // pass all here that was passed in event

                        SetPOI(_tappableGameobjectsInScene[i].GOTapRef.transform);
                        _tappableFocus = _tappableGameobjectsInScene[i];
                    }
                }

            //    if (hitGO.transform == _waterPlane && JoystickStateClosed) ValidWaterTouchEvent.Invoke(_touch, hitPos, dist);
                //    ValidWaterTouchEvent.Invoke(_touch, hitPos, dist);
            }
            else
            {
                SetPOI(null);
                _tappableFocus = null;
            }


            if (_touch.phase == TouchPhase.Ended)
            {
                //
                //   _pointOfInterest.GetComponentInParent<TappableGameobject>().OnTapWasLetGo(_touch, rayPos2D);
                for (int i = 0; i < _tappableGameobjectsInScene.Count; i++)
                {
                    _tappableGameobjectsInScene[i].OnTapWasLetGo();
                }
            }

            // as long as there is touch, fire event
            // used by GOs that need constant touch information, independent from wether or not they are touched (eg. Joystick)
            ValidTouchEvent2D.Invoke(_touch, rayPos2D); 


        }
        else
        {
            for (int i = 0; i < _tappableGameobjectsInScene.Count; i++)
            {
                _tappableGameobjectsInScene[i].OnTapWasLetGo();
            }
        }
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
            if (_lastPOI == _innerJoystick) ValidDoubleTapEvent.Invoke(_touch); //TODO: Change this to joystick event, make new events for other doubletaps or smth

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

        if (_pointOfInterest != null ) _lastPOI = _pointOfInterest;

    }





    private bool CheckForHitOnPlane2D(Touch touch, Plane plane, Transform zValue, out Vector3 rayPos, out GameObject target)
    {
        //transform the touch position into word space from screen space
        Ray mRay = UICamera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, zValue.position.z));
        rayPos = Vector3.zero;

        Vector3 touchPosWorld = UICamera.ScreenToWorldPoint(touch.position);
        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        // this will always get the touch pos in relation to the relevant plane
        if (plane.Raycast(mRay, out float rayDistance))
        {
            rayPos = mRay.GetPoint(rayDistance);
            rayPos = new Vector3(rayPos.x, rayPos.y, zValue.position.z);

        }

        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            return true;
        }
        else
        {
            target = null;
            return false;
        }

    }

    private bool CheckForHitOnPlane(Touch touch, Plane plane, Transform zValue, Transform TransformToHit, out Vector3 worldPosHit, out GameObject target, out float distToCam)
    {
        //transform the touch position into word space from screen space
        Ray mRay = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, zValue.position.z));
        worldPosHit = Vector3.zero;
        target = null;
        distToCam = 0f;

        if (Physics.Raycast(mRay, out RaycastHit h, 1000f))
        {
            if (h.collider.gameObject.transform == TransformToHit)
            {
                distToCam = h.distance;
                
                worldPosHit = Camera.main.WorldToScreenPoint( h.point );
                target = h.collider.gameObject;
                return true;
            }
            else return false;
        }
        else return false;
    }



}
