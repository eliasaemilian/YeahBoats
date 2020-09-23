using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TapEffectCameraZoom : TappableGameobject
{
    [SerializeField] private int _cameraIndex = 0;
    public int CameraIndex { get { return _cameraIndex; } set { _cameraIndex = value; }  }

    [SerializeField] private bool _zoomingIn = true;
    public bool ZoomingIn { get { return _zoomingIn; } set { _zoomingIn = value; } } // <- may not be needed if clear 2D / 3D split remains

    [SerializeField] private GameObject _shopUI = null;
    public GameObject ShopUI { get { return _shopUI; } set { _shopUI = value; } } 

    private TapEffect_CameraHandler _camHandler;

    private void Start()
    {
        _camHandler = FindObjectOfType<TapEffect_CameraHandler>();
        if (_camHandler == null) Debug.LogError("Camera Zoom TapEffect needs a TapEffect CameraHandler in the Scene!");

       if (!ZoomingIn) gameObject.SetActive(false);
    }

    public override void OnTap(Touch touch, Vector3 pos)
    {
        base.OnTap(touch, pos);
        Debug.Log("Got Tapped, changing Camera Index to " + CameraIndex);
        _camHandler.OnCameraChange.Invoke(CameraIndex);

        // I am Return Button bc I am 2D
        gameObject.SetActive(false);
    }

    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        base.OnTap(touch, pos, dist);
        Debug.Log("Got Tapped, changing Camera Index to " + CameraIndex);
        _camHandler.OnCameraChange.Invoke(CameraIndex);

        // Enable UI
        _shopUI.SetActive(true); //TODO: Nice Fade in or smth

        // Disable own Collider
    }
}
