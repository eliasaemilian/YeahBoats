using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_CameraHandler : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (UI_JoystickHandler.JoystickStateChanged == null) Debug.LogError("Boat CameraHandler needs a Joystick in the Scene. Add a JoystickHandler or delete CameraHandler from Boat");
        UI_JoystickHandler.JoystickStateChanged.AddListener(On_UI_InputDetect_Joystick_JoystickEventChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void On_UI_InputDetect_Joystick_JoystickEventChanged()
    {
        if (UI_JoystickHandler.JoystickStateClosed) _anim.SetBool("State_Fishing", true);
        else _anim.SetBool("State_Fishing", false);
    }
}
