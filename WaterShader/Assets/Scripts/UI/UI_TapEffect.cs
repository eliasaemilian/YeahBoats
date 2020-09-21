using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TapEffect : MonoBehaviour, IPooledObject
{
    public Vector3 SpawnPos;
    public float SpawnDistToCam;

    private GameObject _tapEffectPlane;

    private Material _mat_TapPlane;
    private bool _runTapEffect;

    [SerializeField] private float _finalRadius = 3f;
    [SerializeField , Range(0, 1)] private float _finalTransparency = 0f;
    [SerializeField] private float _lerpTime = .8f;

    [SerializeField] private int[] _minMaxDistWaterPlaneToCamera = new int[2]; //min Dist is Camera -> Water at Screen, Max ist furthest into the Waterplane.


    [SerializeField] private float _radius; // start Radius

    private Vector3 _defaultScale;

    private void Awake()
    {
        // Create Object Pool of Tap Planes
        _tapEffectPlane = GetComponentInChildren<MeshRenderer>().gameObject;
        _mat_TapPlane = _tapEffectPlane.GetComponent<MeshRenderer>().material;
        _tapEffectPlane.SetActive(false);


        _defaultScale = _tapEffectPlane.transform.localScale;

    }
    // Start is called before the first frame update
    void Start()
    {
       // UI_InputHandler.ValidWaterTouchEvent.AddListener(OnTapFish_Effect);
    }

    // Update is called once per frame
    void Update()
    {
        if (_runTapEffect)
        {
            StartCoroutine(Fade());
        }
    }

    private void OnTapFish_Effect(Vector3 pos, float dist)
    {
        if (_runTapEffect) return;

        // Set position of Plane to Pos
        _tapEffectPlane.SetActive(true);
        //Vector3 transPos = _uiCamera.ScreenToWorldPoint(pos);
        //_tapEffectPlane.transform.position = new Vector3 (transPos.x, transPos.y, _tapEffectPlane.transform.position.z);
        _runTapEffect = true;
        _counter = 0;

        // get current dist in relation to camera
        dist = Mathfs.Remap(dist, _minMaxDistWaterPlaneToCamera[0], _minMaxDistWaterPlaneToCamera[1], 0, 1);
        dist = Mathf.Clamp01(dist);
        dist = 1 - dist;
        // adjust scale according to dist;
        _tapEffectPlane.transform.localScale = _defaultScale * dist;

        _mat_TapPlane.SetFloat("_Radius", _radius); 
        _mat_TapPlane.SetFloat("_FadeRadius", _radius);
        _mat_TapPlane.SetFloat("_Transparency", .9f);
        
    }

    private float _counter, _lerpRadius, _lerpFadeRadius, _lerpTransparency, t;
    private IEnumerator Fade()
    {
        _counter += Time.deltaTime;
        // Lerp Radius
        _lerpRadius = Mathf.Lerp(_radius, _finalRadius, _counter / _lerpTime);
        _mat_TapPlane.SetFloat("_Radius", _lerpRadius);

        // Lerp Fade Radius
        t = _counter - (_lerpTime * .5f);
        if (t < 0) t = 0;
        _lerpFadeRadius = Mathf.Lerp(_radius, _finalRadius, t / _lerpTime);
        _mat_TapPlane.SetFloat("_FadeRadius", _lerpFadeRadius);

        // Fade in inner Transparency
        t = _counter - (_lerpTime * .167f);
        if (t < 0) t = 0;
        if (_counter > (_lerpTime * .5f)) _finalTransparency = 0f;
        _lerpTransparency = Mathf.Lerp(.9f, _finalTransparency, t / _lerpTime);
        _mat_TapPlane.SetFloat("_Transparency", _lerpTransparency);

        yield return new WaitUntil(() => _counter >= _lerpTime + (_lerpTime * .5f));

        _runTapEffect = false;
        _tapEffectPlane.SetActive(false);
    }

    public void OnInstantiation()
    {
        
    }
       
    public void OnObjectSpawn()
    {
        // do the fade
     //   if (SpawnPos == null || SpawnDistToCam == 0) Debug.LogError("Heck");

        //OnTapFish_Effect(SpawnPos, SpawnDistToCam);
    }

    public void OnObjectDespawn()
    {
        // reset appropriately
    }

    private void OnEnable()
    {
        if (SpawnPos == null || SpawnDistToCam == 0) return;

        OnTapFish_Effect(SpawnPos, SpawnDistToCam);

    }
}
