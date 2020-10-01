using System.Collections;
using System.Collections.Generic;
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
    
}
