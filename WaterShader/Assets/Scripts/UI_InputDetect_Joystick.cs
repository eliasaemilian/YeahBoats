using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UI_InputDetect_Joystick : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;


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
    }

    private Vector3 touchPosWorld;
    private Vector2 touchPosWorld2D;
    private Touch touch;
    void Update()
    {
        // Check for Touches occuring
        touch = Input.GetTouch(0);

        //We check if we have more than one touch happening.
        //We also check if the first touches phase is Ended (that the finger was lifted)
        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended)
        {
            //We transform the touch position into word space from screen space and store it.
            touchPosWorld = _uiCamera.ScreenToWorldPoint(touch.position);
            touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            // Check for Hit with 2D Colliders
            RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hit.collider != null)
            {
                GameObject touchedObject = hit.transform.gameObject;
                Debug.Log("Touched " + touchedObject.transform.name);
            }
        }
    }
}
