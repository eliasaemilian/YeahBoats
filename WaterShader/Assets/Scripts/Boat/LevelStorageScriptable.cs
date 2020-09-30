using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object used to store the cost of various levels
/// </summary>
[CreateAssetMenu(fileName = "New Level Storage", menuName = "Scriptable/Level Storage")]
[System.Serializable]
public class LevelStorageScriptable : ScriptableObject
{
    public List<int> FishingHookCost;
    public List<int> FishingRodCost;
    public List<int> BoatStorageCost;
    public List<int> FishingSpeedCost;
    public List<int> FishermanCost;
}
