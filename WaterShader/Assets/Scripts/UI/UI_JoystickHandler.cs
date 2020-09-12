using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JoystickHandler : MonoBehaviour
{
    private Transform _outerJoystick;
    private Transform _innerJoystick;

    // Start is called before the first frame update
    void Start()
    {
        _outerJoystick = transform;
        _innerJoystick = GetComponentInChildren<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
