using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Settings/PostProcessingSettings/PostProcessingSettings")]
public class PostProcessingSettings : ScriptableObject
{
    public TimeSetting Test;

    // Bloom FOR TESTING LOLO
   // public bool Bloom;
    public PPS_Bloom Bloom;

}
