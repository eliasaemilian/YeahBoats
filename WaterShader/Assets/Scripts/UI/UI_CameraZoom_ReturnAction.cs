using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CameraZoom_ReturnAction : MonoBehaviour
{
    private TapEffect_CameraHandler _camHandler;


    // Start is called before the first frame update
    void Start()
    {
        _camHandler = FindObjectOfType<TapEffect_CameraHandler>();
        if (_camHandler == null) Debug.LogError("Camera Zoom TapEffect needs a TapEffect CameraHandler in the Scene!");

    }


    public void OnReturnButtonClick()
    {
        _camHandler.OnCameraChange.Invoke(0);
    }
}
