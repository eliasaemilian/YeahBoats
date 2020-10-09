using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PostProcessingSettings/Bloom")]
public class PPS_Bloom : ScriptableObject
{
    public bool ThresholdOn;
    public AnimationCurve Threshold;
    public bool IntensityOn;
    public float MaxIntensity;
    public float MinIntensity;
    public AnimationCurve Intensity;
    public bool ScatterOn;
    public AnimationCurve Scatter;
    public bool TintOn;
    public Gradient Tint;
}
