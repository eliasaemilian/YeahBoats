﻿using System;
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
// -> https://www.youtube.com/watch?v=eL_zHQEju8s
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public Material WaterShader;

    public float amplitude = 1f;
    public float length = 20f;
    public float speed = 1f;
    public float offset = 0f;
    public float Timer = 0f;

    public Wave WaveA, WaveB, WaveC;

    public Transform Boat;

 //   public float waveResult;

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



    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed;
        Timer += Time.deltaTime;



    //    Debug.Log(waveResult);

    }

    private void FixedUpdate()
    {
       // waveResult = GerstnerWave(WaveA, Boat.position);
        //waveResult += GerstnerWave(WaveB, Boat.position);
        //waveResult += GerstnerWave(WaveC, Boat.position);
    }

    public float GetWaveHeight (float x) // Get this from Shader
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }

    public float GetWaveHeight() // Get this from Shader
    {
        Vector2 boatPos = new Vector2(Boat.position.x, Boat.position.z);
        float waveResult = GerstnerWave(WaveA, boatPos);
        waveResult += GerstnerWave(WaveB, boatPos);
        waveResult += GerstnerWave(WaveC, boatPos);

        return waveResult;
    }

    private float GerstnerWave(Wave wave, Vector2 p)
    {
    //    float f = wave.k * (Vector2.Dot(wave.Direction, new Vector2 (transform.position.x, transform.position.y)) - wave.c * Timer);

        float _cOffset = wave.c * Timer;
        float _f = wave.k * (Vector2.Dot(wave.Direction, p) - _cOffset);
        float _af = wave.a * Mathf.Cos(_f);
        p.x -= wave.Direction.x * _af; //adjust for approximation of vertex shift along x, z
        p.y -= wave.Direction.y * _af;
        _f = wave.k * (Vector2.Dot(wave.Direction, p) - _cOffset);
        return wave.a * Mathf.Sin(_f); //amplitude
    }
}
