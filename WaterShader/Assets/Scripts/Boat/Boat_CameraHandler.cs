﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_CameraHandler : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        UI_InputHandler.JoystickStateChanged.AddListener(On_UI_InputDetect_Joystick_JoystickEventChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void On_UI_InputDetect_Joystick_JoystickEventChanged()
    {
        if (UI_InputHandler.JoystickStateClosed) _anim.SetBool("State_Fishing", true);
        else _anim.SetBool("State_Fishing", false);
    }
}
