﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_VectorElementScaler : MonoBehaviour
{
    [SerializeField, Range(0,1), Tooltip("Use the Gizmos Sphere to set the position of this GO relative to screenspace")] private float _screenPosX = .5f;
    [SerializeField, Range(0,1), Tooltip("Use the Gizmos Sphere to set the position of this GO relative to screenspace")] private float _screenPosY = .5f;
    [SerializeField] private Camera _uiCamera = null;

    [SerializeField, Tooltip("All Canvas UI that needs to be centered to the same point as this GO, put in this list")] private List<RectTransform> _canvasUIElements = null;


    private void Start()
    {
        if (_uiCamera == null && UI_InputHandler.FetchUICameraInScene() == null) Debug.LogError($"No Camera set or could be found for {gameObject.name}");
        else if (_uiCamera == null) _uiCamera = UI_InputHandler.FetchUICameraInScene();

        // Subscribe to Canvas Scale Resolution Changed Events if relevant to Scene
        if (UI_CanvasResScaler.OnResolutionOrOrientationChanged != null) UI_CanvasResScaler.OnResolutionOrOrientationChanged.AddListener(SetDefaultPosition);

        SetDefaultPosition();
    }

    private void SetDefaultPosition() => SetPositionInScreenSpace(_screenPosX, _screenPosY);

    public void SetPositionInScreenSpace(float xPos, float yPos)
    {
        Vector3 wPos = GetScreenDependendPosition(_uiCamera, xPos, yPos);
        transform.position = wPos;
        if (_canvasUIElements.Count > 0) for (int i = 0; i < _canvasUIElements.Count; i++) _canvasUIElements[i].transform.position = wPos;
    }


    /// <summary>
    /// Will Position this Gameobject as set in the inspector, independent from device resolution.
    /// Use this for GOs that always need to be kept at a certain position in relevance to their camera eg. Vector UI
    /// </summary>
    private Vector3 GetScreenDependendPosition(Camera camera, float xPos, float yPos)
    {
        float x = Mathfs.Remap(xPos, 0, 1, 0, Screen.width);
        float y = Mathfs.Remap(yPos, 0, 1, 0, Screen.height);

        Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(x, y, 0));
        worldPos.z = transform.position.z;

        return worldPos;
    }

    public List<RectTransform> PullCanvasUIElements()
    {
        return _canvasUIElements;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_uiCamera == null) return;

        float x = Mathfs.Remap(_screenPosX, 0, 1, 0, Screen.width);
        float y = Mathfs.Remap(_screenPosY, 0, 1, 0, Screen.height);

        Vector3 worldPos = _uiCamera.ScreenToWorldPoint(new Vector3(x, y, 0));
        worldPos.z = transform.position.z;

        Gizmos.DrawSphere(worldPos, 1f);

        // screen center
        Vector3 center = _uiCamera.ScreenToWorldPoint(new Vector3(Screen.width * .5f, Screen.height * .5f, 0));
        center.z = transform.position.z;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, .5f);
    }
#endif
}
