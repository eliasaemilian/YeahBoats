using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Settings/PostProcessingSettings")]
public class PostProcessingSettings : ScriptableObject
{
    public TimeSetting Test;

    // Bloom
    public bool ThresholdOn;
    public AnimationCurve Threshold;
    public bool IntensityOn;
    public AnimationCurve Intensity;
    public bool ScatterOn;
    public AnimationCurve Scatter;
    public bool TintOn;
    public Gradient Tint;
}
