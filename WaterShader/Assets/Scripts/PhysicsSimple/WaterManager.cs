using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> https://www.youtube.com/watch?v=eL_zHQEju8s


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter _meshF;
    // Start is called before the first frame update
    void Start()
    {
        _meshF = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = _meshF.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.Instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        _meshF.mesh.vertices = vertices;
        _meshF.mesh.RecalculateNormals();
    }
}
