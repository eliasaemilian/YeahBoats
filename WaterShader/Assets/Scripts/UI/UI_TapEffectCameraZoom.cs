using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TapEffectCameraZoom : TappableGameobject
{
    [SerializeField] private float _waitTimeBetweenTappableAgain = 1f;

    [SerializeField] private int _cameraIndex = 0;
    public int CameraIndex { get { return _cameraIndex; } set { _cameraIndex = value; }  }

    [SerializeField] private bool _zoomingIn = true;
    public bool ZoomingIn { get { return _zoomingIn; } set { _zoomingIn = value; } } // <- may not be needed if clear 2D / 3D split remains

    private TapEffect_CameraHandler _camHandler;

    private bool _isLookedAt;

    [SerializeField] private List<GameObject> _UIToSetActive = null;
    [SerializeField] private List<GameObject> _UIToSetInactive = null;


    public override void OnStartInitialize()
    {
        _camHandler = FindObjectOfType<TapEffect_CameraHandler>();
        if (_camHandler == null) Debug.LogError("Camera Zoom TapEffect needs a TapEffect CameraHandler in the Scene!");


        _camHandler.OnCameraChange.AddListener(ReactToCameraChange);
    }


    // 2D UI
    public override void OnTap(Touch touch, Vector3 pos)
    {
        base.OnTap(touch, pos);
        Debug.Log("Got Tapped, changing Camera Index to " + CameraIndex);
        _camHandler.OnCameraChange.Invoke(CameraIndex);

        // I am Return Button bc I am 2D
        gameObject.SetActive(false);
    }

    // 3D Gameobjects
    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        if (_isLookedAt) return;

        base.OnTap(touch, pos, dist);
        //Debug.Log("Got Tapped, changing Camera Index to " + CameraIndex);
        _camHandler.OnCameraChange.Invoke(CameraIndex);

        // Enable UI    
        SetUIVisibility(true);
        

    }

    private IEnumerator WaitUntilTappableAgain()
    {
        yield return new WaitForSeconds(_waitTimeBetweenTappableAgain);
        _isLookedAt = false;
    }

    private void ReactToCameraChange(int index)
    {
        if (!gameObject.activeSelf) return;

        // if we are in the scene and now looked at OR if we were looked at and return button is pressed , set true
        if (index == CameraIndex && _zoomingIn || _isLookedAt && index == 0) _isLookedAt = true;
        else _isLookedAt = false;

        // Index 0 means nothing is looked at anymore and we are returning to the main scene
        if (index == 0) SetUIVisibility(false);

        // give some buffer time so users won't flip back and forth too much with accidental touches
        StartCoroutine(WaitUntilTappableAgain());

    }

    private void SetUIVisibility(bool newState)
    {
        for (int i = 0; i < _UIToSetActive.Count; i++)
        {
            _UIToSetActive[i].SetActive(newState); //TODO: Nice Fade in or smth
        }

        for (int i = 0; i < _UIToSetInactive.Count; i++)
        {
            _UIToSetInactive[i].SetActive(!newState); //TODO: Nice Fade in or smth
        }
    }
}
