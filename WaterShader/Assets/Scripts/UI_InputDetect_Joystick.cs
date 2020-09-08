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

    private bool _inputValid;
    private Vector3 _currentTouchPos;

    Plane objPlane;

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

    }

    private Vector3 touchPosWorld;
    private Vector2 touchPosWorld2D;
    private Touch touch;
    void Update()
    {

        if (Input.touchCount > 0 )
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {


                _currentTouchPos = touchPosWorld;
             
                Debug.Log("Moove");


                //transform the touch position into word space from screen space
                touchPosWorld = _uiCamera.ScreenToWorldPoint(touch.position);
                touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                // Check for Hit with 2D Colliders
                RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward); //not sure why it doesn't accept ui cam tbh :/
                if (hit.collider != null)
                {
                    _currentTouchPos = _uiCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _innerJoystick.position.z));
                    GameObject touchedObject = hit.transform.gameObject;
                //    Debug.Log(touch.position + " and " + touchPosWorld + " and " + Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, hit.transform.position.z)));
                    //   Debug.Log("Touched " + touchedObject.transform.name);
                    _inputValid = true;



                    // NEW
                    Vector3 center = _outerJoystick.GetComponent<MeshRenderer>().bounds.center;
                    center.z = _innerJoystick.position.z;
                    float radius = _outerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _innerJoystick.GetComponent<MeshRenderer>().bounds.extents.x;
                    //Debug.Log("R is " + radius);
                    // Get direction of touch pos in relation to center
                    Vector3 dir = new Vector3 (_currentTouchPos.x, _currentTouchPos.y, _innerJoystick.position.z) - new Vector3 (center.x, center.y, _innerJoystick.position.z);
                    dir = dir.normalized;
                    Debug.DrawLine(center, center + ( dir * radius) , Color.blue, 30f);

                 //   float angRad = t * Mathfs.TAU; // angle in radians   
                  //  Vector2 dir = Mathfs.GetUnitVectorByAngle(angRad);

                    // Move Joystick r * dir
                    Vector3 newPos = center + ( dir * radius ) ;
                    Debug.Log(newPos);
                 //   _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);

                   _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
                    //   _innerJoystick.position = new Vector3(center.x +  radius , _innerJoystick.position.y, _innerJoystick.position.z);

   
                }

                Ray mRay = _uiCamera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, _innerJoystick.position.z));

                float rayDistance;
                Vector3 startPos;
                float radius2 = _outerJoystick.GetComponent<MeshRenderer>().bounds.extents.x - _innerJoystick.GetComponent<MeshRenderer>().bounds.extents.x;

                Vector3 center2 = _outerJoystick.GetComponent<MeshRenderer>().bounds.center;
                center2.z = _innerJoystick.position.z;

                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    startPos = mRay.GetPoint(rayDistance);
                    Debug.Log("swoosh");
                    Vector3 dir = startPos - center2;
                    Vector3 newPos = center2 + (dir.normalized * radius2);
                    //   _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
                    Debug.DrawLine(center2, center2 + startPos.normalized, Color.red, 30f);

                    _innerJoystick.position = new Vector3(newPos.x, newPos.y, _innerJoystick.position.z);
                }

            }


            else if (touch.phase == TouchPhase.Ended)
            {
                _inputValid = false;
           //     Debug.Log("Touch lifted");
            }
        }


    }

    private void FixedUpdate()
    {
        if (_inputValid)
        {
         //   MoveInnerJoystick(touch);

        }
    }
    Vector3 lastPos;
    private void MoveInnerJoystick(Touch t)
    {
        Vector3 newPos = new Vector3(_currentTouchPos.x, _currentTouchPos.y, _innerJoystick.position.z);


        if (lastPos != null && lastPos != newPos)
        {
            float delta = Vector3.Distance(newPos, lastPos);
          //  Debug.Log("Delta is " + delta);
        }


        lastPos = new Vector3(_currentTouchPos.x, _currentTouchPos.y, _innerJoystick.position.z);


        float outerRadius = _outerJoystick.GetComponent<MeshRenderer>().bounds.size.x * .5f;
        // on input found, cache pos
     //   Debug.Log("Current Touch is " + _currentTouchPos);
       // Vector3 newPos = new Vector3(_currentTouchPos.x, _innerJoystick.position.y, _currentTouchPos.z);
      //  Vector3 newPos = new Vector3(_currentTouchPos.x, _currentTouchPos.y, _innerJoystick.position.z);
      //  _innerJoystick.transform.position = Vector3.Lerp(_innerJoystick.transform.position, newPos, _touchSensitivity * Time.deltaTime);
        _innerJoystick.transform.position = _currentTouchPos;
        // lerp towards touch pos while under radius
    }


}
