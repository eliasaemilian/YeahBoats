using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boat_CameraHandler : MonoBehaviour
{
    private Animator _anim;
    private UI_JoystickHandler _joystickHandler;
    private CinemachineStateDrivenCamera _cineCam;

    // Start is called before the first frame update
    void Start()
    {
        _joystickHandler = FindObjectOfType<UI_JoystickHandler>();
        _anim = GetComponent<Animator>();
        if (UI_JoystickHandler.JoystickStateChanged == null) Debug.LogError("Boat CameraHandler needs a Joystick in the Scene. Add a JoystickHandler or delete CameraHandler from Boat");
        UI_JoystickHandler.JoystickStateChanged.AddListener(On_UI_InputDetect_Joystick_JoystickEventChanged);

        _cineCam = FindObjectOfType<CinemachineStateDrivenCamera>();
        if (_cineCam == null) Debug.Log("FUCK");
        _cineCam.Follow = transform;
        _cineCam.LookAt = transform;
    
    }

    /// <summary>
    /// Sets Cinemachine Camera Statemachine Bools depending on user selection on the Joystick
    /// </summary>
    private void On_UI_InputDetect_Joystick_JoystickEventChanged()
    {
        if (_joystickHandler.JoystickStateClosed)
        {
            _anim.SetBool("State_Fishing", true);
        }
        else
        {
            _anim.SetBool("State_Fishing", false);
        }
    }
}
