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
        if (!UI_InputDetect_Joystick.ValidJoystickInput) return;

        if (UI_InputDetect_Joystick.JoystickDirInDegrees > 0 && UI_InputDetect_Joystick.JoystickDirInDegrees < 180) //Accelerate
        {
            Vector2 dir2D = Mathfs.GetUnitVectorByAngle(UI_InputDetect_Joystick.JoystickDirInDegrees * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dir2D.x, 0, dir2D.y);
          //  float xAxis = Mathfs.Remap(UI_InputDetect_Joystick.JoystickDirInDegrees, 0, 180, -1, 1); // X-Axis Input
          //  Vector3 input = new Vector3(xAxis, 1, 0);

         //   Debug.Log(" Degrees " + UI_InputDetect_Joystick.JoystickDirInDegrees + " Indi " + xAxis);
            
         //   Vector3 dir = new Vector3(0, UI_InputDetect_Joystick.JoystickDirInDegrees, 0);
            Quaternion delta = Quaternion.Euler(dir * Time.deltaTime);

            Quaternion dirQ = Quaternion.LookRotation(dir);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * _rSpeed * Time.deltaTime);
            _rb.MoveRotation(slerp);

       //     _rb.AddTorque(dir, ForceMode.Acceleration);
            _rb.AddForce(dir * _speed * Time.deltaTime, ForceMode.Acceleration);
            Debug.Log("Forward");
        }
        else // Slowdown
        {
            _rb.AddForce(Vector3.back * _speed * Time.deltaTime, ForceMode.Acceleration);
            Debug.Log("Reversing");
        }



    }
}
