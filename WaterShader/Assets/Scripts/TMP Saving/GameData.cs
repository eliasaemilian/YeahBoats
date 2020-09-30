using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    // variables to save come here
    public int CurrentMapLevel;
    public int MaxMapLevel;
    public int CurrentBoatLevel;
    public int MaxBoatLevel;
    public int BoatStorageLevel;
    public int Multiplier;
    public int OwnedFishermen;

    public float Money;
    public int Multiplyer;

    //public BoatLevels BoatLevels;

    public GameData( Savedata data)
    {
        CurrentMapLevel = data.CurrentMapLevel;
        MaxMapLevel = data.MaxMapLevel;
        CurrentBoatLevel = data.CurrentBoatLevel;
        MaxBoatLevel = data.MaxBoatLevel;
        BoatStorageLevel = data.BoatStorageLevel;
        Multiplier = data.Multiplier;
        OwnedFishermen = data.OwnedFishermen;
        //BoatLevels = data.BoatLevels;

        Money = data.Money;
        Multiplyer = data.Multiplyer;
    }
}
