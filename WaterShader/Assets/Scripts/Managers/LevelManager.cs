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

    //TODO: split into current and max level
    public int MapLevel;
    public int BoatLevel;
    public int AmmountOfFishermen;

    private int _boatStorageLevel;
    public int BoatStorageLevel { get { return _boatStorageLevel; } set { _boatStorageLevel = value; _boatSkillLevels.BoatStorageLevel = value; } }

    private int _nPCFishermanLevel;
    public int NPCFishermanLevel { get { return _nPCFishermanLevel; } set { _nPCFishermanLevel = value; _boatSkillLevels.NPCFishermanLevel = value; } }

    private int _fishingRodLevel;
    public int FishingRodLevel { get { return _fishingRodLevel; } set { _fishingRodLevel = value; _boatSkillLevels.FishingRodLevel = value; } }

    private int _fishingHookLevel;
    public int FishingHookLevel { get { return _fishingHookLevel; } set { _fishingHookLevel = value; _boatSkillLevels.FishingHookLevel = value; } }

    public int Multiplier;

    public LevelStorageScriptable NPCLevels;
    public LevelStorageScriptable BoatStorageLevels;
    public LevelStorageScriptable FishingRodLevels;
    public LevelStorageScriptable FishingHookLevels;

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

        TMPLevelSetup(false);
        Debug.Log("Boat Level : " + (BoatLevel - 1));
        _boatSkillLevelCosts = LevelCosts.LevelCost[BoatLevel - 1];
    }
    // Start is called before the first frame update
    void Start()
    {
        //For temporary saving
        //GetData();
        //InvokeRepeating("SaveData", 5, 5);
    }


    void OnGUI()
    {
        //if (GUILayout.Button("BoatStorage"))
        //{

        //}
        if (GUI.Button(new Rect(10, 10, 150, 20), "NPC Fisherman Level"))
        {
            if (CheckIfICanLevelup(NPCFishermanLevel, BoatSkillLevelCosts.FishingSpeedCost))
            {
                CanvasDisplay.Instance.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CanvasDisplay.Instance.UpdateText("You need more money!");
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
                CanvasDisplay.Instance.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CanvasDisplay.Instance.UpdateText("You need more money!");
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
                CanvasDisplay.Instance.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CanvasDisplay.Instance.UpdateText("You need more money!");
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
                CanvasDisplay.Instance.UpdateText("You have enough money to upgrade");
            }
            else
            {
                CanvasDisplay.Instance.UpdateText("You need more money!");
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
        MapLevel = 1;
        BoatLevel = 1;
        _nPCFishermanLevel = 1;
        _fishingRodLevel = 1;
        _fishingHookLevel = 1;
        Multiplier = 1;
        _boatStorageLevel = 1;

        if (IsReset)
        {
            MM.ResetMoney();
            UpdateLevels();
        }
    }

    public bool CheckIfICanLevelup(int currentLevel, List<int> LST)
    {
        //TODO: Add level cap
        if (LST.Count < currentLevel) return false;

        if (MM.CheckMoney(LST[currentLevel]))
        {
            Debug.Log("Cost : " + LST[currentLevel-1]);
            return true;
        }
        else return false;

    }
    public void Levelup(ref int currentLevel, List<int> LST, UnityEvent correspondingEvent)
    {
        MM.DeduceMoney(LST[currentLevel-1]);
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
    }
    private void UpdateLevels()
    {
        _boatSkillLevels.BoatStorageLevel = _boatStorageLevel;
        _boatSkillLevels.NPCFishermanLevel = _nPCFishermanLevel;
        _boatSkillLevels.FishingRodLevel = _fishingRodLevel;
        _boatSkillLevels.FishingHookLevel = _fishingHookLevel;
    }

    private void GetData()
    {
        MapLevel = DM.DataContainer.MapLevel;
        BoatLevel = DM.DataContainer.BoatLevel;
        _nPCFishermanLevel = DM.DataContainer.NPCFishermanLevel;
        _fishingRodLevel = DM.DataContainer.FishingRodLevel;
        _fishingHookLevel = DM.DataContainer.FishingHookLevel;
        Multiplier = DM.DataContainer.Multiplier;
        _boatStorageLevel = DM.DataContainer.BoatStorageLevel;
    }

    private void SaveData()
    {
        DM.DataContainer.MapLevel = MapLevel;
        DM.DataContainer.BoatLevel = BoatLevel;
        DM.DataContainer.NPCFishermanLevel = _nPCFishermanLevel;
        DM.DataContainer.FishingRodLevel = _fishingRodLevel;
        DM.DataContainer.FishingHookLevel = _fishingHookLevel;
        DM.DataContainer.Multiplier = Multiplier;
        DM.DataContainer.BoatStorageLevel = _boatStorageLevel;
    }

    
}
