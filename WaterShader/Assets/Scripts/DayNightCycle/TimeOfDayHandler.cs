using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class TimeOfDayHandler : MonoBehaviour
{
    [SerializeField] private bool _useDebugTime;
    [SerializeField, Range(0, 1)] private float _timeOfDayDebug = 0f;

    [SerializeField] private float _timeOfDay; // Range between 0-1

    private Light _sun;

    [SerializeField] private Gradient _SunGradient;
    [SerializeField] private Gradient _AmbientLightGradient;
    [SerializeField] private Gradient _FogGradient;

    void Awake()
    {
        if (_sun == null) _sun = RenderSettings.sun;

        UpdateValuesForTime();
        UpdateGlobalLightingForTimeOfDay();


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

        if (_sun == null && !AttemptToFetchSun())
        {
            Debug.LogError("No Directional Light found in Scene. Place a Directional Light and set it as Sun in Lighting Settings");
            return;
        }

        _sun.color = _SunGradient.Evaluate(_timeOfDay);

        // move sun depending of time of day
        _sun.transform.rotation = Quaternion.Euler(new Vector3((_timeOfDay * 360f) - 90f, 170f, 0f));

    }

    /// <summary>
    /// Will fetch the first directional Light found in scene and regard this light as the sun
    /// </summary>
    /// <returns></returns>
    private bool AttemptToFetchSun()
    {
        Light[] lights = FindObjectsOfType<Light>();
        for (int i = 0; i < lights.Length; i++) if (lights[i].type == LightType.Directional)
            {
                _sun = lights[i];
                return true;
            }

        return false;

    }
}
