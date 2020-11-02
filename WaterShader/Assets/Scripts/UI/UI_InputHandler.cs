using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

[Serializable] public class ValidPlaneTouchEvent2D : UnityEvent<Touch, Vector3> { }
[Serializable] public class ValidPlaneTouchEvent : UnityEvent<Touch, Vector3, float> { }
[Serializable] public class ValidTouchEvent : UnityEvent<Touch> { }

public class UI_InputHandler : MonoBehaviour
{
    public static ValidPlaneTouchEvent2D ValidTouchEvent2D; //UI Box

    public static ValidTouchEvent ValidDoubleTapEvent;


    public static UnityEvent DoubleTapProcessed;

    public Camera UICamera = null;


    [SerializeField] private float _doubleTapSensitivity = .3f;



    private Touch _touch;
    private Plane _uiPlane;



    [SerializeField] private List<TappableGameobject> _tappableGameobjectsInScene;
    private TappableGameobject _tappableFocus;

    // Start is called before the first frame update
    void Awake()
    {
        // SETUP INPUT EVENTS
        if (ValidTouchEvent2D == null) ValidTouchEvent2D = new ValidPlaneTouchEvent2D();

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
            Debug.LogWarning("No UI Camera selected for Input System"); // [!] This will break in a scene with more than 1 overlay camera, camera NEEDS to be set in inspector if that is the case
        }

    }

    void Update() => CheckForTapsOnGameObjects();

    // -> [ https://answers.unity.com/questions/1073979/android-touches-pass-through-ui-elements.html ]
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    Vector3 hitPos, rayPos2D;
    GameObject hitGO;
    /// <summary>
    /// Each Frame the InputHandler checks for any touches occuring on TappableGameObjects
    /// TouchHandler prioritises 2D Colliders over 3D Colliders, therefore ensuring that
    /// 2D Elements used as UI will be preferred over Background Gameobjects
    /// </summary>
    private void CheckForTapsOnGameObjects()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

           // if (IsPointerOverUIObject()) return; //not needed

            for (int i = 0; i < _tappableGameobjectsInScene.Count; i++)
            {
                // 2D
                if (_tappableGameobjectsInScene[i].Tappable2D)
                {
                    // Setup Plane for Touch Input Checks
                    _uiPlane = new Plane(UICamera.transform.forward * -1, _tappableGameobjectsInScene[i].ZValueRef.position);

                    if (CheckForHitOnPlane2D(_touch, _uiPlane, _tappableGameobjectsInScene[i].GOTapRef.transform, out rayPos2D, out hitGO))
                    {

                        // Check for Taps on GOs with 2D Colliders
                        if (_tappableGameobjectsInScene[i].GOTapRef == hitGO)
                        {
                            _tappableGameobjectsInScene[i].OnTap(_touch, rayPos2D); // pass all here that was passed in event

                            // SetPOI(_tappableGameobjectsInScene[i].GOTapRef.transform);
                            _tappableFocus = _tappableGameobjectsInScene[i];
                           
                        }


                    }
                }

                // 3D 

                else if (CheckForHitOnPlane(_touch, _tappableGameobjectsInScene[i].ZValueRef, out hitPos, out hitGO, out float dist))
                {
                    // Check for Taps on GOs with 3D Colliders
                    if (_tappableGameobjectsInScene[i].GOTapRef == hitGO)
                    {
                        _tappableGameobjectsInScene[i].OnTap(_touch, hitPos, dist);

                        //   SetPOI(_tappableGameobjectsInScene[i].GOTapRef.transform);
                        _tappableFocus = _tappableGameobjectsInScene[i];

                    }

                }

                // No Hits occured


                else
                {
                    //  SetPOI(null);
                    _tappableFocus = null;
                }

                // IF TOUCHPHASE ENDED THEN SEND LET GO SIGNAL
                if (_touch.phase == TouchPhase.Ended)
                {
                    _tappableGameobjectsInScene[i].OnTapWasLetGo();

                    if (_tappableFocus == _tappableGameobjectsInScene[i]) RegisterTapForDoubleTap(true, _tappableGameobjectsInScene[i]);
                    else RegisterTapForDoubleTap(false, _tappableGameobjectsInScene[i]);

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


    private void RegisterTapForDoubleTap(bool tapValid, TappableGameobject tappable)
    {
        // DOUBLE TAPPING 
        if (tapValid)
        {
            // On CountDown End: DoubleTap Counters reset
            StartCoroutine(Countdown());

            tappable.TapCount++;
        }
        else
        {
            tappable.TapCount = 0;
            return;
        }

        if (tappable.TapCount == 2)
        {
            //   Debug.Log($"{tappable} got doubleTapped");

            tappable.OnDoubleTap();
            tappable.TapCount = 0;

            StopCoroutine(Countdown());

        }

        //  Debug.Log($"TapCount for {tappable} got changed to {tappable.TapCount}");

    }


    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(_doubleTapSensitivity);

        for (int i = 0; i < _tappableGameobjectsInScene.Count; i++)
        {
            RegisterTapForDoubleTap(false, _tappableGameobjectsInScene[i]);
        }
    }




    public static Camera FetchUICameraInScene()
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        for (int i = 0; i < cameras.Length; i++)
        {
            var cameraData = cameras[i].GetUniversalAdditionalCameraData();
            if (cameraData.renderType == CameraRenderType.Overlay) return cameras[i];
        }

        Debug.LogWarning("No UI Camera (Camera Type: Overlay) could be found in this Scene"); // [!] This will break in a scene with more than 1 overlay camera, camera NEEDS to be set in inspector if that is the case
        return null;
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

    private bool CheckForHitOnPlane(Touch touch, Transform zValue, out Vector3 worldPosHit, out GameObject target, out float distToCam)
    {
        //transform the touch position into word space from screen space
        Ray mRay = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, zValue.position.z));
        worldPosHit = Vector3.zero;
        target = null;
        distToCam = 0f;

        if (Physics.Raycast(mRay, out RaycastHit h, 1000f))
        {
            distToCam = h.distance;

            worldPosHit = Camera.main.WorldToScreenPoint(h.point);
            target = h.collider.gameObject;
            return true;

        }
        else return false;
    }

    public void RegisterTappabeGameObject(TappableGameobject tappable)
    {
        if (_tappableGameobjectsInScene.Count <= 0) _tappableGameobjectsInScene = new List<TappableGameobject>();

        if (!_tappableGameobjectsInScene.Contains(tappable)) _tappableGameobjectsInScene.Add(tappable);
    }
}
