using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves the costs of evey level
/// </summary>
[CreateAssetMenu(fileName = "LevelCosts", menuName = "Scriptable/LevelCosts")]

public class LevelCostsScriptable : ScriptableObject
{
    public List<LevelStorageScriptable> LevelCost;

}
