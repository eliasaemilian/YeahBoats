using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object used to setup different boats and their stats
/// </summary>
[CreateAssetMenu(fileName = "New SmolBoat", menuName = "Scriptable/boat")]
public class BoatScriptable : ScriptableObject
{
    public int MaxLevel;

    // Physics
    public float waterDrag;
    public float waterAngularDrag;
    public float speed;
    public float reversingSpeed;
    public float rotationSpeed;

}
