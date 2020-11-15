using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Inits and Handles the TapEffect Vector Planes when the user is tapping
/// on the Waterplane while Fishing. TapPlanes are pulled and returned to the Object Pool
/// FishingTap Event fires after each tap, alerting Listeners
/// </summary>
public class WaterTappableHandler : TappableGameobject
{
    [SerializeField] private GameObject _tapEffectPrefab = null;
    [SerializeField] private AudioClip _tapEffectSound = null;

    private Camera _uiCamera;
    private UI_JoystickHandler _joystickHandler;

    public static UnityEvent FishingTap = new UnityEvent(); 



    public override void OnStartInitialize()
    {
        _joystickHandler = FindObjectOfType<UI_JoystickHandler>();

        if (FindObjectOfType<UI_JoystickHandler>() == null) Debug.LogError("No Joystick Handler could be found in this Scene. Delete WaterTappable from Waterplane or add a JoystickHandler");
        _uiCamera = FindObjectOfType<UI_InputHandler>().UICamera;

    }

    GameObject spawn;
    Vector3 transPos;
    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        if (!_joystickHandler.JoystickStateClosed) return;

        if (touch.phase == TouchPhase.Began)
        {
            transPos = _uiCamera.ScreenToWorldPoint(pos);
            transPos = new Vector3(transPos.x, transPos.y, ZValueRef.position.z);

            spawn = ObjectPooler.Instance.SpawnFromPool(_tapEffectPrefab.tag, transPos, _tapEffectPrefab.transform.rotation);
            spawn.GetComponent<UI_TapEffect>().SpawnDistToCam = dist; 
            spawn.GetComponent<UI_TapEffect>().SpawnPos = pos;
            spawn.GetComponent<UI_TapEffect>().PoolTag = _tapEffectPrefab.tag;
            spawn.GetComponent<UI_TapEffect>().TriggerTapEffect();
            

            PopupManager.Instance.CallFishAndCoinPopup(transPos + new Vector3(0,1,0));
            FishingTap.Invoke();
            SoundscapeManager.PlaySoundWithClip.Invoke(_tapEffectSound);
        }

    }


}
