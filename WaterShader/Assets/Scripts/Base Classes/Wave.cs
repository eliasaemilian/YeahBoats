using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
