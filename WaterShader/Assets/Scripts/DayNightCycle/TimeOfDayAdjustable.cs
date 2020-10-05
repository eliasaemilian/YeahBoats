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

    public virtual void OnInitialize() { }

    public virtual void OnTimeOfDayValueChange(float newTime) { }
}
