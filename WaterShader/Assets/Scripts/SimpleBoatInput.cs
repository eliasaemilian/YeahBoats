using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class SimpleBoatInput : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] float _speed = 3f;
    [SerializeField] float _rSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (UI_InputDetect_Joystick.JoystickDirInDegrees > 360) return;

        if (UI_InputDetect_Joystick.JoystickDirInDegrees > 0 && UI_InputDetect_Joystick.JoystickDirInDegrees < 180) //Accelerate
        {
            _rb.AddForce(Vector3.forward * _speed * Time.deltaTime, ForceMode.Acceleration);
            Debug.Log("Forward");
        }
        else // Slowdown
        {
            _rb.AddForce(Vector3.back * _speed * Time.deltaTime, ForceMode.Acceleration);
            Debug.Log("Reversing");
        }



    }
}
