﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// LevelManager keeps track of the level of the Boat, Fisherman, Rod etc...
/// </summary>
public class LevelManager : Singleton<LevelManager>
{

    private DataManager dM;
    private MoneyManager mM;
    public LevelCostsScriptable LevelCosts;

    [SerializeField] private MapLevel _mL = new MapLevel();


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

    protected override void Awake()
    {
        base.Awake();
        dM = GameObject.Find("DataManager").GetComponent<DataManager>();
        mM = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
    }
    void Start()
    {
        GetData();
        _boatSkillLevelCosts = LevelCosts.LevelCost[CurrentBoatLevel - 1];
        _independentBoatSkillLevelCosts = LevelCosts.IndependentLevelCosts;
        BoatSkillLevels = BoatLevels.Levels[CurrentBoatLevel - 1];
        SetCurrentMapLevel();

        CatchSpeedMultiplier = 1;
        //For temporary saving
        InvokeRepeating("SaveData", 5, 5);

    }

    /*
    void OnGUI()
    {
        
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
            SaveData();
            dM.DataContainer.Saving();

        }
        if (GUI.Button(new Rect(160, 110, 150, 20), "Unlock The Boats"))
        {
            MaxBoatLevel = 5;
            SaveData();
            dM.DataContainer.Saving();
        }
        if (GUI.Button(new Rect(160, 130, 150, 20), "Unlock The Scenes"))
        {
            MaxMapLevel = 4;
            SaveData();
            dM.DataContainer.Saving();
        }

    }
    */
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
        MaxMapLevel = 1;
        MaxBoatLevel = 1;
        TapCoinLevel = 1;
        TapFishLevel = 1;
        MapPieces = 0;

        if (IsReset)
        {
            mM.ResetMoney();
            UpdateLevels();
            OwnedFishermen = 0;
        }
    }

    public bool CheckIfICanLevelup(int currentLevel, List<int> LST)
    {
        if (LST.Count <= currentLevel) return false;

        if (mM.CheckMoney(LST[currentLevel]))
        {
            return true;
        }
        else return false;

    }
    public bool CheckIfICanLevelupBoat(int currentLevel)
    {
        if (currentLevel == BoatLevels.Levels.Length) return false;
        if (mM == null) Debug.Log("NULL");
        if (mM.CheckMoney(BoatLevels.Levels[currentLevel].Cost))
        {
            return true;
        }
        else return false;
    }
    public int Levelup(int currentLevel, List<int> LST)
    {
        mM.DeduceMoney(LST[currentLevel]);
        
        UpdateLevels();
        //correspondingEvent.Invoke();
        mM.UpdateMoney();
        return (currentLevel + 1);

    }

    public int LevelupBoat(int currentLevel)
    {
        mM.DeduceMoney(BoatLevels.Levels[currentLevel].Cost);

        UpdateLevels();
        mM.UpdateMoney();
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
        CurrentMapLevel = dM.DataContainer.CurrentMapLevel;
        MaxMapLevel = dM.DataContainer.MaxMapLevel;
        MapPieces = dM.DataContainer.MapPieces;
        CurrentBoatLevel = dM.DataContainer.CurrentBoatLevel;
        MaxBoatLevel = dM.DataContainer.MaxBoatLevel;
        Multiplier = dM.DataContainer.Multiplier;
        _boatStorageLevel = dM.DataContainer.BoatStorageLevel;
        _ownedFisherman = dM.DataContainer.OwnedFishermen;
        TapCoinLevel = dM.DataContainer.TapCoinLevel;
        TapFishLevel = dM.DataContainer.TapFishLevel;
        //BoatLevels = DM.DataContainer.BoatLevels;
    }

    public void SaveData()
    {
        dM.DataContainer.CurrentMapLevel = CurrentMapLevel;
        dM.DataContainer.MaxMapLevel = MaxMapLevel;
        dM.DataContainer.MapPieces = MapPieces;
        dM.DataContainer.CurrentBoatLevel = CurrentBoatLevel;
        dM.DataContainer.MaxBoatLevel = MaxBoatLevel;
        dM.DataContainer.Multiplier = Multiplier;
        dM.DataContainer.BoatStorageLevel = _boatStorageLevel;
        dM.DataContainer.OwnedFishermen = OwnedFishermen;
        dM.DataContainer.TapCoinLevel = TapCoinLevel;
        dM.DataContainer.TapFishLevel = TapFishLevel;
        //DM.DataContainer.BoatLevels = BoatLevels;
    }

    public void ReloadBoatUpgrades()
    {
        _boatSkillLevelCosts = LevelCosts.LevelCost[CurrentBoatLevel - 1];
        BoatSkillLevels = BoatLevels.Levels[CurrentBoatLevel - 1];
        SetupLevels();
    }
    public BoatScriptable GetCurretBoatPhysicsSO()
    {
        return BoatLevels.Levels[CurrentBoatLevel - 1].boatScriptable;
    }

    public bool HighSeaboat()
    {
        return CurrentBoatLevel <= 2 ? false : true;
    }
    private void SetCurrentMapLevel()
    {
        switch (_mL)
        {
            case MapLevel.Port:
                break;
            case MapLevel.Pond:
                CurrentMapLevel = 1;
                break;
            case MapLevel.Desert:
                CurrentMapLevel = 3;

                break;
            case MapLevel.Mountains:
                CurrentMapLevel = 2;

                break;
            case MapLevel.Ocean:
                CurrentMapLevel = 4;

                break;
            default:
                break;
        }
    }
}
