using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a Wave used in the WaveManager to calculate the position of the vertices of the waterplane
/// in accordance with the water shader
/// </summary>
public class Wave
{
    public float steepness;
    public float waveLength;

    public float a;
    public float c;
    public float k;
    public Vector3 Direction;


    public Wave(Vector4 waveInput)
    {
        waveLength = waveInput.w;
        steepness = waveInput.z;

        k = Mathfs.TAU / waveLength;
        c = Mathf.Sqrt(9.8f / k);
        Direction = new Vector2(waveInput.x, waveInput.y).normalized;

        a = steepness / k;

    }
}
