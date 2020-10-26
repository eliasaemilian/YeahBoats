using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Savedata : MonoBehaviour
{

    public int CurrentMapLevel = 1;
    public int MaxMapLevel = 1;
    public int MapPieces = 0;
    public int CurrentBoatLevel = 1;
    public int MaxBoatLevel = 1;
    public int BoatStorageLevel = 1;
    public int Multiplier = 1;
    public int OwnedFishermen = 1;
    public int TapCoinLevel = 1;
    public int TapFishLevel = 1;

    public float Money = 0;
    public int Multiplyer = 1;

    public string DateTime;

    //This is the shit that scares me
    public BoatLevels BoatLevels;


    void Awake()
    {
        //GameData data = SaveSystem.LoadData();
        GameData data = SaveSystem.LoadDataJson();

        if (data != null)
        {
            CurrentMapLevel = data.CurrentMapLevel;
            MaxMapLevel = data.MaxMapLevel;
            MapPieces = data.MapPieces;
            CurrentBoatLevel = data.CurrentBoatLevel;
            MaxBoatLevel = data.MaxBoatLevel;
            BoatStorageLevel = data.BoatStorageLevel;
            Multiplier = data.Multiplier;
            OwnedFishermen = data.OwnedFishermen;
            TapCoinLevel = data.TapCoinLevel;
            TapFishLevel = data.TapFishLevel;
            Money = data.Money;
            Multiplyer = data.Multiplyer;
            DateTime = data.DateTime;
            //BoatLevels = data.BoatLevels;
        }
        else
        {
            //SaveSystem.SaveData(this);
            SaveSystem.SaveDataJson(this);
        }
        InvokeRepeating("Saving", 6, 5);
    }

    private void Saving()
    {
        //SaveSystem.SaveData(this);
        SaveSystem.SaveDataJson(this);
        Debug.Log("Saving ...");
    }
}
