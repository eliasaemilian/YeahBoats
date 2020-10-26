using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Camera))]
public class WaterRipplesEffect : MonoBehaviour
{
    [SerializeField] RenderTexture rt = null;
    [SerializeField] Transform target = null;

    // Start is called before the first frame update
    void Awake()
    {
        Shader.SetGlobalTexture("_WaterRipplesRT", rt);
        Shader.SetGlobalFloat("_WaterRipplesOrthoCamSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Shader.SetGlobalVector("_WaterRipplesPos", transform.position);
    }
}
