using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TapEffect : MonoBehaviour
{
    public GameObject TapEffectPlane;

    private Material _mat_TapPlane;
    private bool _runTapEffect;

    [SerializeField] private float _finalRadius = 3f;
    [SerializeField , Range(0, 1)] private float _finalTransparency = 0f;
    [SerializeField] private float _lerpTime = .8f;


    private void Awake()
    {
        // Create Object Pool of Tap Planes
        _mat_TapPlane = GetComponent<MeshRenderer>().material;
        TapEffectPlane.SetActive(false);


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
            Debug.Log("Tap Effect");
        }
    }

    private void OnTapFish_Effect(Touch _touch, Vector3 pos)
    {
        // Set position of Plane to Pos
        TapEffectPlane.SetActive(true);
        TapEffectPlane.transform.position = new Vector3 (pos.x, pos.y, TapEffectPlane.transform.position.z);
        _runTapEffect = true;
        _counter = 0;
        _mat_TapPlane.SetFloat("_Radius", 0f);
    }

    private float _counter, _lerpRadius, _lerpTransparency;
    private IEnumerator Fade()
    {
        _counter += Time.deltaTime;
        // Lerp outer Radius
        _lerpRadius = Mathf.Lerp(0, _finalRadius, _counter / _lerpTime);
        _mat_TapPlane.SetFloat("_Radius", _lerpRadius);

        // Fade in inner Transparency
        _lerpTransparency = Mathf.Lerp(.8f, _finalTransparency, _counter / _lerpTime);
        _mat_TapPlane.SetFloat("_Transparency", _lerpTransparency);

        yield return new WaitUntil(() => _counter >= _lerpTime);

        _runTapEffect = false;
        TapEffectPlane.SetActive(false);
    }
}
