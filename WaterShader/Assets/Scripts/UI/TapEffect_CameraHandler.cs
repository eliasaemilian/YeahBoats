using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class CameraChangeEvent : UnityEvent<int> { }
public class TapEffect_CameraHandler : MonoBehaviour
{
    public CameraChangeEvent OnCameraChange = new CameraChangeEvent();

    private Animator _anim;

    void Awake()
    {
        if (OnCameraChange != null) OnCameraChange.AddListener(OnCameraChangeEvent);
        else Debug.LogError("No Camera Change Event setup");

        _anim = GetComponent<Animator>();
    }


    private void OnCameraChangeEvent(int index)
    {
        _anim.SetInteger("CameraIndex", index);
        Debug.Log("Setting Camera to " + index);
    }
}
