using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayAdjustable : MonoBehaviour
{
    private Light _light;

    // put in SO eventually I gueess mayhaps
    [SerializeField] private float _maxIntensity;

    [SerializeField] private AnimationCurve _lightIntensity;
    [SerializeField] private Gradient _lightColor;

    private TimeOfDayHandler _timeHandler;

    private void Start()
    {
        _light = GetComponent<Light>();
        _timeHandler = FindObjectOfType<TimeOfDayHandler>();
        _timeHandler.TimeOfDayIsUpdated.AddListener(OnTimeOfDayValueChange);

        if (_light == null) _light = GetComponentInChildren<Light>();
        if (_light == null) Debug.LogError("There is no Light Attached to this GameObject, hecking do et D:<");
    }

    public virtual void OnTimeOfDayValueChange(float newTime)
    {
        Debug.Log("Updating Lights");

        // Everything that needs to react to the time of day goes here
        _light.intensity = _maxIntensity * _lightIntensity.Evaluate(newTime);
        _light.color = _lightColor.Evaluate(newTime);
    }
}
