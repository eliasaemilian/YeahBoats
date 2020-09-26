using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object used to store the cost of various levels
/// </summary>
[CreateAssetMenu(fileName = "New Level Storage", menuName = "Scriptable/Level Storage")]
public class LevelStorageScriptable : ScriptableObject
{

    public LevelScriptable[] Levels;
    public List<int> FishingHookCost;
    public List<int> FishingRodCost;
    public List<int> BoatStorageCost;
    public List<int> FishingSpeedCost;
}
