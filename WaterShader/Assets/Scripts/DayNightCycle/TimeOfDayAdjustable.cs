using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeOfDayAdjustable : MonoBehaviour
{
    private TimeOfDayHandler _timeHandler;

    private void Start()
    {
        _timeHandler = FindObjectOfType<TimeOfDayHandler>();
        _timeHandler.TimeOfDayIsUpdated.AddListener(OnTimeOfDayValueChange);

        OnInitialize();
    }

    /// <summary>
    /// Gets called at Start of Scene for all TimeOfDayAdjustables
    /// </summary>
    public virtual void OnInitialize() { }

    /// <summary>
    /// Gets called for all TimeOfDayAdjustables whenever a new change in Time of Day gets evaluated eg. at Application restart
    /// </summary>
    public virtual void OnTimeOfDayValueChange(float newTime) { }
}
