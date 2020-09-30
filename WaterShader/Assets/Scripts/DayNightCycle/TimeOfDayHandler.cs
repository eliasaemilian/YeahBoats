using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEditor.UIElements;
using UnityEngine;

public class TimeOfDayHandler : MonoBehaviour
{
    [SerializeField] private bool _useDebugTime;
    [SerializeField, Range(0, 1)] private float _timeOfDayDebug = 0f;

    [SerializeField] private float _timeOfDay; // Range between 0-1

    private float _currentSunY;
    private Light _sun;

    private bool _isNight;

    [SerializeField] private Light _moon;
    [SerializeField] private float _moonMinIntensityValue;
    [SerializeField] private float _moonMaxIntensityValue;

    //[SerializeField] private Gradient _AmbientLightGradient;
    //[SerializeField] private Gradient _FogGradient;

    [SerializeField] private Gradient _SunGradient;
    [SerializeField] private float _sunMinIntensityValue;
    [SerializeField] private float _sunMaxIntensityValue;
    [SerializeField] private AnimationCurve _sunIntensity;

    [SerializeField] private Gradient _MoonGradient;
    [SerializeField] private CurveField _moonIntensity;

    void Awake()
    {
        if (_sun == null && !AttemptToFetchSun())
        {
            Debug.LogError("No Directional Light found in Scene. Place a Directional Light and set it as Sun in Lighting Settings");
            return;
        }
        else _currentSunY = _sun.transform.rotation.eulerAngles.y;
        
        //UpdateValuesForTime();
        //UpdateGlobalLightingForTimeOfDay();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateValuesForTime();
        UpdateGlobalLightingForTimeOfDay();
    }

    private void UpdateValuesForTime()
    {
        // get current time on scale between 0 - 1
        int sysHours = System.DateTime.Now.Hour;
        int sysMin = System.DateTime.Now.Minute;
        if (sysMin > 30) sysHours++;
        _timeOfDay = Mathf.Clamp01(sysHours / 24f);

#if UNITY_EDITOR
        if (_useDebugTime) _timeOfDay = _timeOfDayDebug;
#endif

       
    }

    private void UpdateGlobalLightingForTimeOfDay()
    {
        // change light colors 
        //RenderSettings.ambientLight = _AmbientLightGradient.Evaluate(_timeOfDay); // seems to do fuck all these days, maky :/
        //RenderSettings.fogColor = _FogGradient.Evaluate(_timeOfDay);
        Debug.Log("Setting sun for Time: " + _timeOfDay);



        _sun.color = _SunGradient.Evaluate(_timeOfDay);
        _moon.color = _MoonGradient.Evaluate(_timeOfDay);


        // move sun & moon depending of time of day
        float x = Mathf.Lerp(0, 180, _timeOfDay);
        _sun.transform.rotation = Quaternion.Euler(new Vector3(x, _currentSunY, 0f));
        _moon.transform.rotation = Quaternion.Euler(new Vector3(x, _currentSunY - 180, 0f));


        float sunIntensity = Mathf.Clamp01(_sunIntensity.Evaluate(_timeOfDay));
        float moonIntensity = Mathf.Abs(sunIntensity - 1);

        _sun.intensity = Mathf.Lerp(_sunMinIntensityValue, _sunMaxIntensityValue, sunIntensity);
        _moon.intensity = Mathf.Lerp(_moonMinIntensityValue, _moonMaxIntensityValue, moonIntensity);

        if (_sun.intensity <= 0.05) RenderSettings.sun = _moon;
        else RenderSettings.sun = _sun;



    }

    /// <summary>
    /// Sets Sun to Sun choosen in Lighting Settings, if no Sun set:
    /// Will fetch the first directional Light found in scene and regard this light as the sun
    /// </summary>
    /// <returns></returns>
    private bool AttemptToFetchSun()
    {
        // if Sun is set in Lighting Settings, choose this
        _sun = RenderSettings.sun;
        if (_sun != null) return true;

        // else attempt to find a directional Light in the Scene.
        Light[] lights = FindObjectsOfType<Light>();
        for (int i = 0; i < lights.Length; i++) if (lights[i].type == LightType.Directional)
            {
                _sun = lights[i];
                return true;
            }

        return false;

    }
}
