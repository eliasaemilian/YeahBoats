using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Every 2D or 3D Object in the Scene that is not Unity UI and should react to Touches will need to inherit from
/// Tappable GameObject.
/// Tappable GO will delegate to OnTap() when the GO was tapped, OnTapWasLetGo() when touches have been lifted and 
/// OnDoubleTap() if a double tap was registered for this GO
/// Everything that needs to be setup for this GO at the Start of Scene needs to run in OnStartInitialize()
/// _sceneStartActiveStatus will set the GO active/inactive after initialization at the Start of the Scene
/// </summary>
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
        CheckForRegistrationWithInputHandler();
        OnStartInitialize();
        gameObject.SetActive(_sceneStartActiveStatus);
    }

    public virtual void OnStartInitialize() { }

    private void SetupTappable(GameObject refGO, bool is2D)
    {
        Tappable2D = is2D;
        GOTapRef = refGO;
    }

    public virtual void OnDoubleTap() { }

    // for GOs with a 2D Collider
    public virtual void OnTap(Touch touch, Vector3 pos) { }

    // for GOs with a 3D Collider
    public virtual void OnTap(Touch touch, Vector3 pos, float dist) { }

    public virtual void OnTapWasLetGo() { }


    private void CheckForRegistrationWithInputHandler()
    {
        UI_InputHandler inputHandler = FindObjectOfType<UI_InputHandler>();
        if (inputHandler != null) inputHandler.RegisterTappabeGameObject(this);
        else Debug.LogError("No Input Handler can be found in Scene. Tappable GameObjects won't be processed");
    }
}
