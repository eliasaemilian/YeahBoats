using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [ -> https://forum.unity.com/threads/canvashelper-resizes-a-recttransform-to-iphone-xs-safe-area.521107/ ]
/// <summary>
/// Adjusts UI Element positions for the phones specific resolution and SafeArea
/// to make sure they are always within the SafeArea at the specified location
/// </summary>
[RequireComponent(typeof (RectTransform))]
public class UI_CanvasResScaler : MonoBehaviour
{
    public static UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();

    private static bool _screenChangeVarsInitialized = false;
    private static ScreenOrientation _lastOrientation = ScreenOrientation.Landscape;
    private static Vector2 _lastResolution = Vector2.zero;
    private static Rect _lastSafeArea = Rect.zero;

    private RectTransform _rect;
    private Canvas _canvas;

    // Start is called before the first frame update
    void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rect = GetComponent<RectTransform>();

        _rect.anchorMin = new Vector2(0,0);
        _rect.anchorMax = new Vector2(1,1);

        if (!_screenChangeVarsInitialized) // get screen params on startup
        {
            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            _lastSafeArea = Screen.safeArea;

            _screenChangeVarsInitialized = true;
        }


        SetResolution();
    }


    void OnRectTransformDimensionsChange() //like lmao I dont even think this is relevant BUT IF SOME FUCKER EVER CHANGES THEIR RESOLUTION IM FUCKING WATCHING
    {
        SetResolution();
    }

    private void SetResolution()
    {
        if (_rect == null) return;

      //  Debug.Log("Resolution got set");
        //   bool verticalOrientation = _rect.rect.width < _rect.rect.height ? true : false; this would get orientation if needed, or use device orientation

        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= _canvas.pixelRect.width;
        anchorMin.y /= _canvas.pixelRect.height;
        anchorMax.x /= _canvas.pixelRect.width;
        anchorMax.y /= _canvas.pixelRect.height;

        _rect.anchorMin = anchorMin;
        _rect.anchorMax = anchorMax;
    }
}
