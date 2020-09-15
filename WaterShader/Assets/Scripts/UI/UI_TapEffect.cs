using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TapEffect : MonoBehaviour
{
    private GameObject _tapEffectPlane;

    private Material _mat_TapPlane;
    private bool _runTapEffect;

    [SerializeField] private float _finalRadius = 3f;
    [SerializeField] private float _finalFadeRadius = 1.5f;
    [SerializeField , Range(0, 1)] private float _finalTransparency = 0f;
    [SerializeField] private float _lerpTime = .8f;

    [SerializeField] private int[] _minMaxDistWaterPlaneToCamera = new int[2]; //min Dist is Camera -> Water at Screen, Max ist furthest into the Waterplane.

    private Camera _uiCamera;

    private void Awake()
    {
        // Create Object Pool of Tap Planes
        _tapEffectPlane = GetComponentInChildren<MeshRenderer>().gameObject;
        _mat_TapPlane = _tapEffectPlane.GetComponent<MeshRenderer>().material;
        _tapEffectPlane.SetActive(false);

        _uiCamera = FindObjectOfType<UI_InputHandler>().UICamera;



    }
    // Start is called before the first frame update
    void Start()
    {
        UI_InputHandler.ValidWaterTouchEvent.AddListener(OnTapFish_Effect);
    }

    // Update is called once per frame
    void Update()
    {
        if (_runTapEffect)
        {
            StartCoroutine(Fade());
        }
    }

    private void OnTapFish_Effect(Touch _touch, Vector3 pos, float dist)
    {
        if (_runTapEffect) return;

        // Set position of Plane to Pos
        _tapEffectPlane.SetActive(true);
        Vector3 transPos = _uiCamera.ScreenToWorldPoint(pos);
        _tapEffectPlane.transform.position = new Vector3 (transPos.x, transPos.y, _tapEffectPlane.transform.position.z);
        _runTapEffect = true;
        _counter = 0;
        _mat_TapPlane.SetFloat("_Radius", 1.4f); // <- these must be put in in unity
        _mat_TapPlane.SetFloat("_FadeRadius", 1.4f);
        _mat_TapPlane.SetFloat("_Transparency", .9f);

        // get current dist in relation to camera
        dist = Mathfs.Remap(dist, _minMaxDistWaterPlaneToCamera[0], _minMaxDistWaterPlaneToCamera[1], 0, 1);
        dist = Mathf.Clamp01(dist);
       
        // Change all other values according to dist

    }

    private float _counter, _lerpRadius, _lerpFadeRadius, _lerpTransparency, t;
    private IEnumerator Fade()
    {
        Debug.Log("Tap Effect");

        _counter += Time.deltaTime;
        // Lerp Radius
        _lerpRadius = Mathf.Lerp(1.4f, _finalRadius, _counter / _lerpTime);
        _mat_TapPlane.SetFloat("_Radius", _lerpRadius);

        // Lerp Fade Radius
        t = _counter - .3f;
        if (t < 0) t = 0;
        _lerpFadeRadius = Mathf.Lerp(1.4f, _finalRadius, t / _lerpTime);
        _mat_TapPlane.SetFloat("_FadeRadius", _lerpFadeRadius);

        // Fade in inner Transparency
        t = _counter - .1f;
        if (t < 0) t = 0;
        if (_counter > (_lerpTime * .5f)) _finalTransparency = 0f; //TODO: change all of these to percentages
        _lerpTransparency = Mathf.Lerp(.9f, _finalTransparency, t / _lerpTime);
        _mat_TapPlane.SetFloat("_Transparency", _lerpTransparency);

        yield return new WaitUntil(() => _counter >= _lerpTime + .5f);

        _runTapEffect = false;
        _tapEffectPlane.SetActive(false);
    }
}
