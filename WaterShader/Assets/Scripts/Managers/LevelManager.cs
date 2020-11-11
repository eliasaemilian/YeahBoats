using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// LevelManager keeps track of the level of the Boat, Fisherman, Rod etc...
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public DataManager DM;
    public MoneyManager MM;
    public LevelCostsScriptable LevelCosts;

    // Just for testing
    public CanvasDisplay CD;

    //Boat Independent
    public int CurrentMapLevel;
    public int MaxMapLevel;
    public int MapPieces;

    public int CurrentBoatLevel;
    public int MaxBoatLevel;

    private int _tapCoinLevel;
    public int TapCoinLevel { get { return _tapCoinLevel; } set { _tapCoinLevel = value; } }

    private int _tapFishLevel;
    public int TapFishLevel { get { return _tapFishLevel; } set { _tapFishLevel = value; } }

    private int _ownedFisherman;
    public int OwnedFishermen { get { return _ownedFisherman; } set { _ownedFisherman = value; _boatSkillLevels.NPCFishermanAmmount = value; } }

    //Boat dependent
    public int MaxAmmountOfFishermen;

    private int _boatStorageLevel;
    public int BoatStorageLevel { get { return _boatStorageLevel; } set { _boatStorageLevel = value; _boatSkillLevels.BoatStorageLevel = value; } }

    private int _nPCFishermanLevel;
    public int NPCFishermanLevel { get { return _nPCFishermanLevel; } set { _nPCFishermanLevel = value; _boatSkillLevels.NPCFishermanLevel = value; } }

    private int _fishingRodLevel;
    public int FishingRodLevel { get { return _fishingRodLevel; } set { _fishingRodLevel = value; _boatSkillLevels.FishingRodLevel = value; } }

    private int _fishingHookLevel;
    public int FishingHookLevel { get { return _fishingHookLevel; } set { _fishingHookLevel = value; _boatSkillLevels.FishingHookLevel = value; } }

    private int _multiplier;
    public int Multiplier { get { return _multiplier; } set { _multiplier = value; MultiplierUpdate.Invoke(); } }
    public float CatchSpeedMultiplier;

    public BoatLevels BoatLevels;

    private LevelStorageScriptable _boatSkillLevelCosts = null;
    public LevelStorageScriptable BoatSkillLevelCosts { get { return _boatSkillLevelCosts; } set { _boatSkillLevelCosts = value; } }

    private LevelStorageScriptable _independentBoatSkillLevelCosts = null;
    public LevelStorageScriptable IndependentBoatSkillLevelCosts { get { return _independentBoatSkillLevelCosts; } set { _independentBoatSkillLevelCosts = value; } }

    private LevelScriptable _boatSkillLevels = null;
    public LevelScriptable BoatSkillLevels{ get { return _boatSkillLevels; } set { _boatSkillLevels = value; SetupLevels(); } }

    //still needed for now
    public UnityEvent NPCUpdate;
    public UnityEvent MultiplierUpdate;
    //deprecated
    public UnityEvent BoatStorageUpdate;
    public UnityEvent FishingRodUpdate;
    public UnityEvent FishingHookUpdate;
    void Awake()
    {
        Instance = this;

        //TMPLevelSetup(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetData();
        _boatSkillLevelCosts = LevelCosts.LevelCost[CurrentBoatLevel - 1];
        _independentBoatSkillLevelCosts = LevelCosts.IndependentLevelCosts;
        BoatSkillLevels = BoatLevels.Levels[CurrentBoatLevel - 1];

        CatchSpeedMultiplier = 1;
        //For temporary saving
        InvokeRepeating("SaveData", 5, 5);

    }


    void OnGUI()
    {
        //if (GUILayout.Button("BoatStorage"))
        //{

        //}
        
        if (GUI.Button(new Rect(160, 50, 150, 20), "Delete Saves"))
        {
            if(File.Exists(Application.persistentDataPath + "/Data.txt"))
            {
                File.Delete(Application.persistentDataPath + "/Data.txt");
                Debug.Log("files deleted");
            }
            else
            {
                
                Debug.Log("nothing to delete");
            }
        }
        
        if (GUI.Button(new Rect(160, 90, 150, 20), "ResetLevels"))
        {
            TMPLevelSetup(true);
            UpdateLevels();
            DM.DataContainer.Saving();

        }
        if (GUI.Button(new Rect(160, 110, 150, 20), "Unlock The Boats"))
        {
            MaxBoatLevel = 4;
            UpdateLevels();
            DM.DataContainer.Saving();
        }

    }
    private void TMPLevelSetup(bool IsReset)
    {
        CurrentMapLevel = 1;
        CurrentBoatLevel = 1;
        _nPCFishermanLevel = 1;
        _fishingRodLevel = 1;
        _fishingHookLevel = 1;
        Multiplier = 1;
        _boatStorageLevel = 1;
        MaxAmmountOfFishermen = 1;
        MaxBoatLevel = 1;
        TapCoinLevel = 1;
        TapFishLevel = 1;
        MapPieces = 0;

        if (IsReset)
        {
            MM.ResetMoney();
            UpdateLevels();
            OwnedFishermen = 1;
        }
    }

    public bool CheckIfICanLevelup(int currentLevel, List<int> LST)
    {
        //TODO: Add level cap
        if (LST.Count < currentLevel) return false;

        if (MM.CheckMoney(LST[currentLevel]))
        {
            return true;
        }
        else return false;

    }
    public bool CheckIfICanLevelupBoat(int currentLevel)
    {
        if (currentLevel == BoatLevels.Levels.Length) return false;
        if (MM.CheckMoney(BoatLevels.Levels[currentLevel].Cost))
        {
            return true;
        }
        else return false;
    }
    public int Levelup(int currentLevel, List<int> LST)
    {
        MM.DeduceMoney(LST[currentLevel]);
        
        UpdateLevels();
        //correspondingEvent.Invoke();
        MM.UpdateMoney();
        return (currentLevel + 1);

    }

    public int LevelupBoat(int currentLevel)
    {
        MM.DeduceMoney(BoatLevels.Levels[currentLevel].Cost);

        UpdateLevels();
        MM.UpdateMoney();
        return (currentLevel + 1);
    }

    public bool ChanceLevelupForFisherman(int chanceInPercentage)
    {
        int rnd = UnityEngine.Random.Range(1, 101);
        if(chanceInPercentage >= rnd) return true;
        else return false;
        
    }

    public bool CheckIfPubHasUpgrades()
    {        
        if (IndependentBoatSkillLevelCosts != null && CheckIfICanLevelup(OwnedFishermen, IndependentBoatSkillLevelCosts.FishermanCost))
        {
            return true;
        }
        else return false;
    }
    public bool CheckIfShackHasUpgrades()
    {
        if (BoatSkillLevelCosts == null) return false;

        if (CheckIfICanLevelup(FishingHookLevel, BoatSkillLevelCosts.FishingHookCost) || 
            CheckIfICanLevelup(FishingRodLevel, BoatSkillLevelCosts.FishingRodCost) ||
            CheckIfICanLevelup(TapCoinLevel, BoatSkillLevelCosts.TapCoinCost) ||
            CheckIfICanLevelup(TapFishLevel, BoatSkillLevelCosts.TapFishCost))
        {
            return true;
        }
        else return false;
    }
    public bool CheckIfBoatShackHasUpgrades()
    {
        if (CheckIfICanLevelupBoat(MaxBoatLevel))
        {
            return true;
        }
        else return false;
    }
    public bool CheckIfLighthouseHasUpgrades()
    {
        if (MapPieces == 4)
        {
            return true;
        }
        else return false;
    }
    public void AddMapPiece()
    {
        MapPieces++;
    }
    //update the levels when loading SO
    private void SetupLevels()
    {
        _boatStorageLevel = _boatSkillLevels.BoatStorageLevel;
        _nPCFishermanLevel = _boatSkillLevels.NPCFishermanLevel;
        _fishingRodLevel = _boatSkillLevels.FishingRodLevel;
        _fishingHookLevel = _boatSkillLevels.FishingHookLevel;
        MaxAmmountOfFishermen = _boatSkillLevels.NPCFishermanAmmount;
        
    }
    private void UpdateLevels()
    {
        _boatSkillLevels.BoatStorageLevel = _boatStorageLevel;
        _boatSkillLevels.NPCFishermanLevel = _nPCFishermanLevel;
        _boatSkillLevels.FishingRodLevel = _fishingRodLevel;
        _boatSkillLevels.FishingHookLevel = _fishingHookLevel;
        _boatSkillLevels.NPCFishermanAmmount = _ownedFisherman;
    }

    private void GetData()
    {
        CurrentMapLevel = DM.DataContainer.CurrentMapLevel;
        MaxMapLevel = DM.DataContainer.MaxMapLevel;
        MapPieces = DM.DataContainer.MapPieces;
        CurrentBoatLevel = DM.DataContainer.CurrentBoatLevel;
        MaxBoatLevel = DM.DataContainer.MaxBoatLevel;
        Multiplier = DM.DataContainer.Multiplier;
        _boatStorageLevel = DM.DataContainer.BoatStorageLevel;
        _ownedFisherman = DM.DataContainer.OwnedFishermen;
        TapCoinLevel = DM.DataContainer.TapCoinLevel;
        TapFishLevel = DM.DataContainer.TapFishLevel;
        //BoatLevels = DM.DataContainer.BoatLevels;
    }

    public void SaveData()
    {
        DM.DataContainer.CurrentMapLevel = CurrentMapLevel;
        DM.DataContainer.MaxMapLevel = MaxMapLevel;
        DM.DataContainer.MapPieces = MapPieces;
        DM.DataContainer.CurrentBoatLevel = CurrentBoatLevel;
        DM.DataContainer.MaxBoatLevel = MaxBoatLevel;
        DM.DataContainer.Multiplier = Multiplier;
        DM.DataContainer.BoatStorageLevel = _boatStorageLevel;
        DM.DataContainer.OwnedFishermen = OwnedFishermen;
        DM.DataContainer.TapCoinLevel = TapCoinLevel;
        DM.DataContainer.TapFishLevel = TapFishLevel;
        //DM.DataContainer.BoatLevels = BoatLevels;
    }

    
}
