using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // variables to save come here
    public int MapLevel;
    public int BoatLevel;
    public int BoatStorageLevel;
    public int NPCFishermanLevel;
    public int FishingRodLevel;
    public int FishingHookLevel;
    public int Multiplier;

    public float Money;
    public int Multiplyer;
    public GameData( Data data)
    {
        MapLevel = data.MapLevel;
        BoatLevel = data.BoatLevel;
        BoatStorageLevel = data.BoatStorageLevel;
        NPCFishermanLevel = data.NPCFishermanLevel;
        FishingRodLevel = data.FishingRodLevel;
        FishingHookLevel = data.FishingHookLevel;
        Multiplier = data.Multiplier;

        Money = data.Money;
        Multiplyer = data.Multiplyer;
    }
}
