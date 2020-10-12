using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class ServicesInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!RuntimeManager.IsInitialized()) RuntimeManager.Init();
        Debug.Log("Init completed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
