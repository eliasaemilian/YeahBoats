using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Saves data
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public LevelManager _lM;

    public Savedata DataContainer;

    void Awake()
    {
        Instance = this;
        
        DataContainer = GetComponent<Savedata>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //_lM = LevelManager.Instance;

    }

}
