using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : TappableGameobject, IPooledObject
{
    public CrateSpawner CS;

    public override void OnStartInitialize()
    {
        base.OnStartInitialize();
        //I need help dunno what to do
        ObjectPooler.Instance.ReturnToPool("Crate", this.gameObject);

    }
    public void OnInstantiation()
    {
       // throw new System.NotImplementedException();
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
}
