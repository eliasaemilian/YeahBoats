using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savedata : MonoBehaviour
{

    public int MapLevel = 1;
    public int BoatLevel = 1;
    public int BoatStorageLevel = 1;
    public int NPCFishermanLevel = 1;
    public int FishingRodLevel = 1;
    public int FishingHookLevel = 1;
    public int Multiplier = 1;

    public float Money = 0;
    public int Multiplyer = 1;



    void Awake()
    {
        GameData data = SaveSystem.LoadData();
        if (data != null)
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
