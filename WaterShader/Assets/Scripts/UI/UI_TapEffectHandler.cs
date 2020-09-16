using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UI_TapEffectHandler : MonoBehaviour
{
    private ObjectPooler _objPool;
    [SerializeField] private GameObject _tapEffectPrefab;
    private Camera _uiCamera;

    [SerializeField] private Transform _tapEffectZValueReference;

    // Start is called before the first frame update
    void Start()
    {
        UI_InputHandler.ValidWaterTouchEvent.AddListener(OnTapSpawnFromPool);
        _objPool = ObjectPooler.Instance;
        _uiCamera = FindObjectOfType<UI_InputHandler>().UICamera;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTapSpawnFromPool(Touch _touch, Vector3 pos, float dist)
    {
        if (_touch.phase == TouchPhase.Began)
        {
            Vector3 transPos = _uiCamera.ScreenToWorldPoint(pos);
            transPos = new Vector3(transPos.x, transPos.y, _tapEffectZValueReference.position.z);

            GameObject spawn = ObjectPooler.Instance.SpawnFromPool(_tapEffectPrefab.tag, transPos, _tapEffectPrefab.transform.rotation);
            spawn.GetComponent<UI_TapEffect>().SpawnDistToCam = dist;
            spawn.GetComponent<UI_TapEffect>().SpawnPos = pos;
            Debug.Log("SPAWNED A TAPEFFECT " + spawn.name);
            spawn.SetActive(true);
        }

    }


}
