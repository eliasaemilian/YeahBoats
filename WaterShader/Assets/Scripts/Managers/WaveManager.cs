using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wave
{
    public float steepness;
    public float waveLength;

    public float a;
    public float c;
    public float k;
    public Vector3 Direction;


    public Wave (Vector4 waveInput)
    {
        waveLength = waveInput.w;
        steepness = waveInput.z;

        k = Mathfs.TAU / waveLength;
        c = Mathf.Sqrt(9.8f / k);
        Direction = new Vector2 (waveInput.x, waveInput.y).normalized;

        a = steepness / k;

    }
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public Material WaterShader;

    public float Timer = 0f;

    public Wave WaveA, WaveB, WaveC;
    public float WaveSpeed;

    public Transform WaterPlane; //MUST BE X,Z = 0

    public float WaveHeightResult;

    private Vector3 _waterPos;
    private Vector3 _waterScale;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Generate Waves from Shader for Approximation
        WaveA = new Wave(WaterShader.GetVector("_WaveA"));
        WaveB = new Wave(WaterShader.GetVector("_WaveB"));
        WaveC = new Wave(WaterShader.GetVector("_WaveC"));
        WaveSpeed = WaterShader.GetFloat("_WaveSpeed"); //NOT ACCOUNTED FOR

        _waterPos = WaterPlane.position; _waterScale = WaterPlane.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
    }


    public float GetWaveHeight(Vector3 pos) // from Shader
    {
        Vector2 p = new Vector2(pos.x, pos.z);
        p /= _waterScale;

        float waveResult = GerstnerWave(WaveA, p);
        waveResult += GerstnerWave(WaveB, p);
        waveResult += GerstnerWave(WaveC, p);

        WaveHeightResult = _waterPos.y + ( waveResult * _waterScale.y);
        return WaveHeightResult;
    }

    private float GerstnerWave(Wave wave, Vector2 p)
    {
        float _cOffset = wave.c * Timer ; //TODO: Add WaveSpeed
        float _f = wave.k * (Vector2.Dot(wave.Direction, p) - _cOffset);
        float _af = wave.a * Mathf.Sin(_f);
        p.x -= ( wave.Direction.x / _waterScale.x ) * _af; //adjust for approximation of vertex shift along x, z
        p.y -= ( wave.Direction.y / _waterScale.y ) * _af;
        _f = wave.k * (Vector2.Dot(wave.Direction, p) - _cOffset);
        return wave.a * Mathf.Cos(_f); //amplitude
    }

}
