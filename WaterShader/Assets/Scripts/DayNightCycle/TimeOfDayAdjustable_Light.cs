using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayAdjustable_Light : TimeOfDayAdjustable
{
    private Light _light;

    // put in SO eventually I gueess mayhaps
    [SerializeField] private float _maxIntensity = 1f;

    [SerializeField] private AnimationCurve _lightIntensity = null;
    [SerializeField] private Gradient _lightColor = null;


    public override void OnInitialize()
    {
        _light = GetComponent<Light>();


        if (_light == null) _light = GetComponentInChildren<Light>();
        if (_light == null) Debug.LogError("There is no Light Attached to this GameObject, hecking do et D:<");
    }

    public override void OnTimeOfDayValueChange(float newTime)
    {
        Debug.Log("Updating Lights");

        // Everything that needs to react to the time of day goes here
        _light.intensity = _maxIntensity * _lightIntensity.Evaluate(newTime);
        _light.color = _lightColor.Evaluate(newTime);
    }
}
