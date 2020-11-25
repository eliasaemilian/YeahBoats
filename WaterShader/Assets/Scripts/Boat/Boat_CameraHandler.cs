using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boat_CameraHandler : MonoBehaviour
{
    private Animator _anim;
    private UI_JoystickHandler _joystickHandler;
    private CinemachineStateDrivenCamera _cineCam;

    private Transform _cineCamFocusPoint, _cineCamFocusPointFishing;

    private string _tagFocusPoint = "CamFocusPoint", _tagFocusPointFishing = "CamFocusPointFishing";

    // Start is called before the first frame update
    void Start()
    {
        _joystickHandler = FindObjectOfType<UI_JoystickHandler>();
        _anim = GetComponent<Animator>();
        if (UI_JoystickHandler.JoystickStateChanged == null) Debug.LogError("Boat CameraHandler needs a Joystick in the Scene. Add a JoystickHandler or delete CameraHandler from Boat");
        UI_JoystickHandler.JoystickStateChanged.AddListener(On_UI_InputDetect_Joystick_JoystickEventChanged);

        InitCinemachineCameraSetup();
    }

    /// <summary>
    /// Sets Cinemachine Camera Statemachine Bools depending on user selection on the Joystick
    /// </summary>
    private void On_UI_InputDetect_Joystick_JoystickEventChanged()
    {
        if (_joystickHandler.JoystickStateClosed)
        {
            _anim.SetBool("State_Fishing", true);

            SetCameraFocusPoint(_cineCamFocusPointFishing);

        }
        else
        {
            _anim.SetBool("State_Fishing", false);

            SetCameraFocusPoint(_cineCamFocusPoint);

        }
    }

    private Transform CollectCamFocusPoint(string tag)
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].CompareTag(tag)) return children[i];
        }

        return null;
    }

    private void SetCameraFocusPoint(Transform focus)
    {
        if (_cineCam == null) Debug.Log("Frick D:");

        _cineCam.Follow = focus;
        _cineCam.LookAt = focus;
    }

    /// <summary>
    /// Collect Boat specific focus points to use for Cinemachine Cameras
    /// </summary>
    private void InitCinemachineCameraSetup()
    {
        _cineCam = FindObjectOfType<CinemachineStateDrivenCamera>();

        _cineCamFocusPoint = transform; _cineCamFocusPointFishing = transform;
        if (CollectCamFocusPoint(_tagFocusPoint) != null) _cineCamFocusPoint = CollectCamFocusPoint(_tagFocusPoint);
        if (CollectCamFocusPoint(_tagFocusPointFishing) != null) _cineCamFocusPointFishing = CollectCamFocusPoint(_tagFocusPointFishing);

        SetCameraFocusPoint(_cineCamFocusPoint);
        _cineCam.m_AnimatedTarget = _anim;


        if (LevelManager.Instance.CurrentBoatLevel >= 3)
        {
            CinemachineVirtualCamera[] vCams = FindObjectsOfType<CinemachineVirtualCamera>();
            for (int i = 0; i < vCams.Length; i++)
            {
                vCams[i].m_Lens.FieldOfView = 60f;
            }
        }
    }
}
