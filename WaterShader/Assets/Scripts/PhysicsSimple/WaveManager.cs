﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> https://www.youtube.com/watch?v=eL_zHQEju8s
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight (float x) // Get this from Shader
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }
}
