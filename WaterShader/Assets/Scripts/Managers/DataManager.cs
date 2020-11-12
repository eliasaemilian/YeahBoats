using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Saves data
/// </summary>
public class DataManager : Singleton<DataManager>
{
    public Savedata DataContainer;

    protected override void Awake()
    {
        base.Awake();
        DataContainer = GetComponent<Savedata>();
    }
    
}
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    [SerializeField] bool dontDestroyOnLoad;

    public static T Instance { get => instance; }
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            instance = (T)this;
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Second instance of " + GetType());
            Destroy(this);
        }
    }
}
