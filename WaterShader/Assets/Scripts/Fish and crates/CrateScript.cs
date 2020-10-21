﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : TappableGameobject, IPooledObject
{
    public CrateSpawner CS;

    public override void OnStartInitialize()
    {
        base.OnStartInitialize();
        Debug.Log("Crate init");
        //I need help dunno what to do
     //   ObjectPooler.Instance.ReturnToPool("Crate", this.gameObject);

    }
    public void OnInstantiation()
    {
        // throw new System.NotImplementedException();
        OnInitialize();
    }

    public void OnObjectDespawn()
    {
       // throw new System.NotImplementedException();
    }

    public void OnObjectSpawn()
    {
        //throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        CS.m_CrateEvent.Invoke(this.gameObject);
    }

    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        base.OnTap(touch, pos, dist);
        Debug.Log("I was tapped");
    }
}
