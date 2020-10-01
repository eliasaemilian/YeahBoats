using System;
using System.Collections;
using System.Collections.Generic;
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

    public int CurrentBoatLevel;
    public int MaxBoatLevel;
    private int _ownedFisherman;
    public int OwnedFishermen { get { return _ownedFisherman; } set { _ownedFisherman = value; _boatSkillLevels.NPCFishermanAmmount = value; } }

    //Boat dependent
    public int MaxAmmountOfFishermen;

    private int _boatStorageLevel;
    public int BoatStorageLevel { get { return _boatStorageLevel; } set { _boatStorageLevel = value; _boatSkillLevels.BoatStorageLevel = value; } }

    private int _nPCFishermanLevel;
    public int NPCFishermanLevel { get { return _nPCFishermanLevel; } set { _nPCFishermanLevel = value; _boatSkillLevels.NPCFishermanLevel = value; } }

    public int _fishingRodLevel;
    public int FishingRodLevel { get { return _fishingRodLevel; } set { _fishingRodLevel = value; _boatSkillLevels.FishingRodLevel = value; } }

    public int _fishingHookLevel;
    public int FishingHookLevel { get { return _fishingHookLevel; } set { _fishingHookLevel = value; _boatSkillLevels.FishingHookLevel = value; } }

    public int Multiplier;

    public BoatLevels BoatLevels;

    private LevelStorageScriptable _boatSkillLevelCosts = null;
    public LevelStorageScriptable BoatSkillLevelCosts { get { return _boatSkillLevelCosts; } set { _boatSkillLevelCosts = value; } }

    private LevelScriptable _boatSkillLevels = null;
    public LevelScriptable BoatSkillLevels{ get { return _boatSkillLevels; } set { _boatSkillLevels = value; SetupLevels(); } }


    public UnityEvent NPCUpdate;
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
        BoatSkillLevels = BoatLevels.Levels[CurrentBoatLevel - 1];

        //For temporary saving
        InvokeRepeating("SaveData", 5, 5);
    }


    void OnGUI()
    {
        //if (GUILayout.Button("BoatStorage"))
        //{

        //}
        if (GUI.Button(new Rect(10, 10, 150, 20), "NPC Fisherman Level"))
        {
            if (CheckIfICanLevelup( NPCFishermanLevel, BoatSkillLevelCosts.FishingSpeedCost))
            {
                CD.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CD.UpdateText("You need more money!");
            }
        }
        if (GUI.Button(new Rect(160, 10, 150, 20), "Upgrade"))
        {
            Debug.Log("About to upgrade");
            Levelup(ref _nPCFishermanLevel, BoatSkillLevelCosts.FishingSpeedCost, NPCUpdate);
        }

        if (GUI.Button(new Rect(10, 30, 150, 20), "Boat Storage"))
        {
            if (CheckIfICanLevelup(BoatStorageLevel, BoatSkillLevelCosts.BoatStorageCost))
            {
                CD.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CD.UpdateText("You need more money!");
            }

        }
        if (GUI.Button(new Rect(160, 30, 150, 20), "Upgrade"))
        {
            Levelup(ref _boatStorageLevel, BoatSkillLevelCosts.BoatStorageCost, BoatStorageUpdate);
        }
        if (GUI.Button(new Rect(10, 50, 150, 20), "Fishing Hook Level"))
        {
            if (CheckIfICanLevelup(FishingHookLevel, BoatSkillLevelCosts.FishingHookCost))
            {
                CD.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CD.UpdateText("You need more money!");
            }

        }
        if (GUI.Button(new Rect(160, 50, 150, 20), "Upgrade"))
        {
            Levelup(ref _fishingHookLevel, BoatSkillLevelCosts.FishingHookCost, FishingHookUpdate);
        }
        if (GUI.Button(new Rect(10, 70, 150, 20), "Fishing Rod Level"))
        {
            if (CheckIfICanLevelup(FishingRodLevel, BoatSkillLevelCosts.FishingRodCost))
            {
                CD.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CD.UpdateText("You need more money!");
            }
        }
        if (GUI.Button(new Rect(160, 70, 150, 20), "Upgrade"))
        {
            Levelup(ref _fishingRodLevel, BoatSkillLevelCosts.FishingRodCost, FishingRodUpdate);
        }
        if (GUI.Button(new Rect(160, 90, 150, 20), "ResetLevels"))
        {
            TMPLevelSetup(true);
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
    public void Levelup(ref int currentLevel, List<int> LST, UnityEvent correspondingEvent)
    {
        MM.DeduceMoney(LST[currentLevel]);
        currentLevel++;
        UpdateLevels();
        correspondingEvent.Invoke();

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
        CurrentBoatLevel = DM.DataContainer.CurrentBoatLevel;
        MaxBoatLevel = DM.DataContainer.MaxBoatLevel;
        Multiplier = DM.DataContainer.Multiplier;
        _boatStorageLevel = DM.DataContainer.BoatStorageLevel;
        _ownedFisherman = DM.DataContainer.OwnedFishermen;
        //BoatLevels = DM.DataContainer.BoatLevels;
    }

    private void SaveData()
    {
        DM.DataContainer.CurrentMapLevel = CurrentMapLevel;
        DM.DataContainer.MaxMapLevel = MaxMapLevel;
        DM.DataContainer.CurrentBoatLevel = CurrentBoatLevel;
        DM.DataContainer.MaxBoatLevel = MaxBoatLevel;
        DM.DataContainer.Multiplier = Multiplier;
        DM.DataContainer.BoatStorageLevel = _boatStorageLevel;
        DM.DataContainer.OwnedFishermen = OwnedFishermen;
        //DM.DataContainer.BoatLevels = BoatLevels;
    }

    
}
