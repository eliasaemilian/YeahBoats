﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class SimpleBoatInput : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] float _speed = 3f;
    [SerializeField] float _reversingSpeed = .8f;
    [SerializeField] float _rSpeed = 10f;


    [SerializeField] bool _reversedInput = false; // TODO: [Refactoring] Move to Settings -> store in PlayerPrefs

    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

   
    void FixedUpdate() => CalculateBoatMovementFromJoystickInput();

    float speed;
    private void CalculateBoatMovementFromJoystickInput()
    {
        if (!UI_InputDetect_Joystick.ValidJoystickInput) return;

        // Calculate Move Towards Direction and Apply Force & Rotation
        Vector2 dir2D = Mathfs.GetUnitVectorByAngle(UI_InputDetect_Joystick.JoystickDirInDegrees * Mathf.Deg2Rad);
        Vector3 dir = new Vector3(dir2D.x, 0, dir2D.y);

        if (_reversedInput) dir = -dir;

        if (UI_InputDetect_Joystick.JoystickDirInDegrees > 0 && UI_InputDetect_Joystick.JoystickDirInDegrees < 180) speed = _speed; //Accelerate
        else speed = -_reversingSpeed; //Reverse 

        Quaternion dirQ = Quaternion.LookRotation(dir);
        Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * _rSpeed * Time.deltaTime);

        _rb.MoveRotation(slerp);
        _rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Acceleration);
    }
}