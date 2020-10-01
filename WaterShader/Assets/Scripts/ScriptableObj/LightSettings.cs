using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu (menuName = "Settings/LightSettings")]
public class LightSettings : ScriptableObject
{


    //[SerializeField] private Gradient _AmbientLightGradient;
    //[SerializeField] private Gradient _FogGradient;

    public Gradient SunGradient;
    public float SunMinIntensityValue;
    public float SunMaxIntensityValue;
    public AnimationCurve SunIntensity;

    public Gradient MoonGradient;
    public float MoonMinIntensityValue;
    public float MoonMaxIntensityValue;
    public CurveField MoonIntensity;

    public Gradient SkyBoxColorSky;
    public Gradient SkyBoxColorGround;
}
