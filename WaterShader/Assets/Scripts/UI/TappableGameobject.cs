using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TappableGameobject : MonoBehaviour, ITappable
{
    public bool Tappable2D { get; set; }
    public GameObject GOTapRef { get; set; }

    [SerializeField] private Transform _zValueRef = null;
    public Transform ZValueRef { get { return _zValueRef; } set { _zValueRef = value; } } // Set to UI Plane if this has a corresponding Effect, else set to whichever GO needs the tap position

    [HideInInspector] public int TapCount { get; set; }

    [SerializeField, Tooltip("Make sure GO is active in Scene or it will not be found by InputHandler")] private bool _sceneStartActiveStatus = true; // true, GO will be set to active at start of Scene
    public void OnInitialize()
    {
        // Check if GO has a Collider attached
        if (GetComponent<Collider>() != null) SetupTappable(gameObject, false);
        else if (GetComponentInChildren<Collider>() != null) SetupTappable(GetComponentInChildren<Collider>().gameObject, false);
        else if (GetComponent<Collider2D>() != null) SetupTappable(gameObject, true);
        else if (GetComponentInChildren<Collider2D>() != null) SetupTappable(GetComponentInChildren<Collider2D>().gameObject, true);
        else Debug.LogError("Tappable Gameobjects need at least one 2D or 3D Collider attached to their Gameobject or Children");

        if (_zValueRef == null)
        {
            _zValueRef = transform;
            Debug.LogWarning($"No ZValue has been set in Inspector for {gameObject.name}. Therefore ZValue wil be set to {gameObject.name}.");
        }

        // Irregardless of Scene Setup make sure GO is properly initialized
        gameObject.SetActive(true);
        OnStartInitialize();
        gameObject.SetActive(_sceneStartActiveStatus);
    }

    public virtual void OnStartInitialize() { }

    private void SetupTappable(GameObject refGO, bool is2D)
    {
        Tappable2D = is2D;
        GOTapRef = refGO;
    }
    public virtual void OnDoubleTap()
    {
       // Debug.Log(gameObject.name + " got double Tapped");

    }


    // for GOs with a 2D Collider
    public virtual void OnTap(Touch touch, Vector3 pos)
    {
       // Debug.Log(gameObject.name + " got Tapped, am 2D");
    }

    // for GOs with a 3D Collider
    public virtual void OnTap(Touch touch, Vector3 pos, float dist)
    {
       // Debug.Log(gameObject.name + " got Tapped, am 3D");
    }



    public virtual void OnTapWasLetGo()
    {
      //  Debug.Log("Tap was let go");
    }
}
