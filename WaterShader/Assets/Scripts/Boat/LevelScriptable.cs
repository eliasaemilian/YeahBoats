using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Scriptable object used to store the cost of various levels
/// </summary>
[CreateAssetMenu(fileName = "New Level", menuName = "Scriptable/Level")]
public class LevelScriptable : ScriptableObject
{
    
    public int Cost;
    [Header("Use this if Boat")]
    public GameObject BoatPrefab;
    public LevelStorageScriptable BoatSkillsLevels;

    public int BoatStorageLevel = 1;
    public int NPCFishermanLevel = 1;
    public int FishingRodLevel = 1;
    public int FishingHookLevel = 1;
}
