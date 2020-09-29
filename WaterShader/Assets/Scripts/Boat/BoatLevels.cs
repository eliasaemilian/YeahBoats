using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This SO saves the indivisual Boat levels
/// </summary>
[CreateAssetMenu(fileName = "BoatLevels", menuName = "Scriptable/BoatLevels")]
public class BoatLevels : ScriptableObject
{
    public LevelScriptable[] Levels;
}
