using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public float a;
    public float c;
    public float k;
    public Vector3 Direction;
}
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

    private float GerstnerWave(Wave _wave, Vector2 _p)
    {
        float _cOffset = _wave.c * offset;
        float _f = _wave.k * (Vector2.Dot(_wave.Direction, _p) - _cOffset);
        float _af = _wave.a * Mathf.Cos(_f);
        _p.x -= _wave.Direction.x * _af;
        _p.y -= _wave.Direction.y * _af;
        _f = _wave.k * (Vector2.Dot(_wave.Direction, _p) - _cOffset);
        return _wave.a * Mathf.Sin(_f);
    }
}
