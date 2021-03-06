﻿using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))] [RequireComponent (typeof (BoatBase))]
public class SimpleBoatInput : MonoBehaviour
{
    private bool ReverseInput
    {
        get
        {
            if (SettingsHandler.RequestSetting(SettingsHandler.ReverseInput, out float reverseInput))
            {
                if (reverseInput > 0) return false;
                else return true;
            }
            else return false;           
        }
    } 

    private float _speed = 3f;
    private float _reversingSpeed = .8f;
    private float _rotationSpeed = 10f;

    private Rigidbody _rb;
    private BoatScriptable _boatSO;
    private UI_JoystickHandler _joystickHandler;

    void Start()
    {
        _joystickHandler = FindObjectOfType<UI_JoystickHandler>();
        _rb = GetComponent<Rigidbody>();
        _boatSO = GetComponent<BoatBase>().BoatSO;
        _speed = _boatSO.speed;
        _reversingSpeed = _boatSO.reversingSpeed;
        _rotationSpeed = _boatSO.rotationSpeed;
    }

   
    void FixedUpdate() => CalculateBoatMovementFromJoystickInput();

    float speed;
    /// <summary>
    /// Takes Joystick Input in Degree and translates this to movement & rotation direction for the current boat
    /// </summary>
    private void CalculateBoatMovementFromJoystickInput()
    {
        if (!UI_JoystickHandler.ValidJoystickInput || _joystickHandler.JoystickStateClosed) return;

        // Calculate Move Towards Direction and Apply Force & Rotation
        Vector2 dir2D = Mathfs.GetUnitVectorByAngle(UI_JoystickHandler.JoystickDirInDegrees * Mathf.Deg2Rad);
        Vector3 dir = new Vector3(dir2D.x, 0, dir2D.y);

        if (ReverseInput) dir = -dir;

        dir = transform.TransformDirection(dir);

        if (UI_JoystickHandler.JoystickDirInDegrees > 0 && UI_JoystickHandler.JoystickDirInDegrees < 180) speed = _speed; //Accelerate
        else speed = -_reversingSpeed; //Reverse 

        Quaternion dirQ = Quaternion.LookRotation(dir);
        Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * _rotationSpeed * Time.deltaTime);

        _rb.MoveRotation(slerp);
        _rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Acceleration);
    }
}
