using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object used to setup different boats and their stats
/// </summary>
[CreateAssetMenu(fileName = "New SmolBoat", menuName = "Scriptable/boat")]
public class BoatScriptable : ScriptableObject
{
    public string Name;
    public int Level;
    public int Storage;
    public int NPCSpawnPointAmmount;

}
