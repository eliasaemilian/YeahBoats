using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string Tag;
    public GameObject Prefab;
    public int Size => (int)(NumberOfObjects * Uniqueness);

    public int NumberOfObjects; // total Number in Pool
    [Range(0, 1), Tooltip("Only use Uniqueness of < 1 for Meshes that are generated at runtime to improve performance")]
    public float Uniqueness; // 1 = all unique, 0 generates only 1 

    public Transform PooledObjectParent;

    // Start is called before the first frame update

}
