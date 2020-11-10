using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    private Material WaterShader; // <- TODO GET FROM WATER PLANE

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
        WaterShader = WaterPlane.GetComponent<MeshRenderer>().material;


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

    /// <summary>
    /// Calculate WaveHeight at a given position according to the same
    /// calculations run for the water shader
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetWaveHeight(Vector3 pos)
    {
        Vector2 p = new Vector2(pos.x, pos.z);
        p /= _waterScale;

        float waveResult = GerstnerWave(WaveA, p);
        waveResult += GerstnerWave(WaveB, p);
        waveResult += GerstnerWave(WaveC, p);

        WaveHeightResult = _waterPos.y + ( waveResult * _waterScale.y);
        return WaveHeightResult;
    }

    /// <summary>
    /// Single Gerstner Wave Calculation
    /// Same as Custom Node GernsterWaves for the WaterShader
    /// </summary>
    /// <param name="wave">Wave Params for calculated Wave</param>
    /// <param name="p">Position on the Waterplane</param>
    /// <returns></returns>
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
