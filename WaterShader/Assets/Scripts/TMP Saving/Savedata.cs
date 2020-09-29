﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savedata : MonoBehaviour
{

    public int CurrentMapLevel = 1;
    public int MaxMapLevel = 1;
    public int CurrentBoatLevel = 1;
    public int MaxBoatLevel = 1;
    public int BoatStorageLevel = 1;
    public int Multiplier = 1;
    public int OwnedFishermen = 1;

    public float Money = 0;
    public int Multiplyer = 1;

    public LevelStorageScriptable BoatLevels;


    void Awake()
    {
        GameData data = SaveSystem.LoadData();
        if (data != null)
        {
            CurrentMapLevel = data.CurrentMapLevel;
            MaxMapLevel = data.MaxMapLevel;
            CurrentBoatLevel = data.CurrentBoatLevel;
            MaxBoatLevel = data.MaxBoatLevel;
            BoatStorageLevel = data.BoatStorageLevel;
            Multiplier = data.Multiplier;
            OwnedFishermen = data.OwnedFishermen;
            Money = data.Money;
            Multiplyer = data.Multiplyer;
            BoatLevels = data.BoatLevels;
        }
        else
        {
            SaveSystem.SaveData(this);
        }
        InvokeRepeating("Saving", 6, 5);
    }

    private void Saving()
    {
        SaveSystem.SaveData(this);
        Debug.Log("Saving ...");
    }
}
