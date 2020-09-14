using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Scriptable object used to store the cost of various levels
/// </summary>
[CreateAssetMenu(fileName = "New Level", menuName = "Scriptable/Level")]
public class LevelScriptable : ScriptableObject
{
    public int Level;
    public int Cost;
    [Header("Use this if Boat")]
    public GameObject BoatPrefab;
}
