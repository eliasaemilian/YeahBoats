using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaterTappableHandler : TappableGameobject
{
    [SerializeField] private GameObject _tapEffectPrefab = null;

    private Camera _uiCamera;


    // Start is called before the first frame update
    void Start()
    {


    }

    public override void OnStartInitialize()
    {
        if (FindObjectOfType<UI_JoystickHandler>() == null) Debug.LogError("No Joystick Handler could be found in this Scene. Delete WaterTappable from Waterplane or add a JoystickHandler");
        _uiCamera = FindObjectOfType<UI_InputHandler>().UICamera;
    }

    GameObject spawn;
    Vector3 transPos;
    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        if (!UI_JoystickHandler.JoystickStateClosed) return;

        if (touch.phase == TouchPhase.Began)
        {
            transPos = _uiCamera.ScreenToWorldPoint(pos);
            transPos = new Vector3(transPos.x, transPos.y, ZValueRef.position.z);

            spawn = ObjectPooler.Instance.SpawnFromPool(_tapEffectPrefab.tag, transPos, _tapEffectPrefab.transform.rotation);
            spawn.GetComponent<UI_TapEffect>().SpawnDistToCam = dist; //TODO: david get this lesbian plant outta here
            spawn.GetComponent<UI_TapEffect>().SpawnPos = pos;
            spawn.SetActive(true);
        }

    }


}
