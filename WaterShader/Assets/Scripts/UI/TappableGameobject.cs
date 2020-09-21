using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TappableGameobject : MonoBehaviour, ITappable
{
    public bool Tappable2D { get; set; }
    public GameObject GOTapRef { get; set; }

    public delegate void AddToEventListener();

    public void OnInitialize()
    {
        // Check if GO has a Collider attached
        if (GetComponent<Collider>() != null) SetupTappable(gameObject, false);
        else if (GetComponentInChildren<Collider>() != null) SetupTappable(GetComponentInChildren<Collider>().gameObject, false);
        else if (GetComponent<Collider2D>() != null) SetupTappable(gameObject, true);
        else if (GetComponentInChildren<Collider2D>() != null) SetupTappable(GetComponentInChildren<Collider2D>().gameObject, true);
        else Debug.LogError("Tappable Gameobjects need at least one 2D or 3D Collider attached to their Gameobject or Children");
    }

    private void SetupTappable(GameObject refGO, bool is2D)
    {
        Tappable2D = is2D;
        GOTapRef = refGO;
    }
    public void OnDoubleTap()
    {
        throw new System.NotImplementedException();
    }


    // for GOs with a 2D Collider
    public virtual void OnTap(Touch touch, Vector3 pos)
    {
        Debug.Log(gameObject.name + " got Tapped, am 2D");
    }

    // for GOs with a 3D Collider
    public virtual void OnTap(Touch touch, Vector3 pos, float dist)
    {
        Debug.Log(gameObject.name + " got Tapped, am 3D");
    }



    public virtual void OnTapWasLetGo()
    {
        Debug.Log("Tap was let go");
    }
}
