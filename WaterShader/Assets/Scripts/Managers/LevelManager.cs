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

    public DataManager _dM;
    public MoneyManager _mM;

    // Just for testing
    public CanvasDisplay CD;

    public int MapLevel;
    public int BoatLevel;
    public int BoatStorageLevel;
    public int NPCFishermanLevel;
    public int FishingRodLevel;
    public int FishingHookLevel;
    public int Multiplier;

    public LevelStorageScriptable NPCLevels;
    public LevelStorageScriptable BoatStorageLevels;
    public LevelStorageScriptable FishingRodLevels;
    public LevelStorageScriptable FishingHookLevels;

    public UnityEvent NPCUpdate;
    public UnityEvent BoatStorageUpdate;
    public UnityEvent FishingRodUpdate;
    public UnityEvent FishingHookUpdate;
    void Awake()
    {
        Instance = this;
        
        TMPLevelSetup();
    }
    // Start is called before the first frame update
    void Start()
    {
        //_dM = DataManager.Instance;
        //_mM = MoneyManager.Instance;

        //For temporary saving
        GetData();
        InvokeRepeating("SaveData", 5, 5);
    }


    void OnGUI()
    {
        //if (GUILayout.Button("BoatStorage"))
        //{

        //}
        if (GUI.Button(new Rect(10, 10, 150, 20), "NPC Fisherman Level"))
        {
            if(CheckIfICanLevelup(ref NPCFishermanLevel, NPCLevels))
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
            Levelup(ref NPCFishermanLevel, NPCLevels, NPCUpdate);
        }

        if (GUI.Button(new Rect(10, 30, 150, 20), "Boat Storage"))
        {
            if(CheckIfICanLevelup(ref BoatStorageLevel, BoatStorageLevels))
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
            Levelup(ref BoatStorageLevel, BoatStorageLevels, BoatStorageUpdate);
        }
        if (GUI.Button(new Rect(10, 50, 150, 20), "Fishing Hook Level"))
        {
            if(CheckIfICanLevelup(ref FishingHookLevel, FishingHookLevels))
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
            Levelup(ref FishingHookLevel, FishingHookLevels, FishingHookUpdate);
        }
        if (GUI.Button(new Rect(10, 70, 150, 20), "Fishing Rod Level"))
        {
            if(CheckIfICanLevelup(ref FishingRodLevel, FishingRodLevels))
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
            Levelup(ref FishingRodLevel, FishingRodLevels, FishingRodUpdate);
        }
        if (GUI.Button(new Rect(160, 90, 150, 20), "ResetLevels"))
        {
            TMPLevelSetup();
        }

    }
    private void TMPLevelSetup()
    {
        MapLevel = 1;
        BoatLevel = 1;
        NPCFishermanLevel = 1;
        FishingRodLevel = 1;
        FishingHookLevel = 1;
        Multiplier = 1;
        BoatStorageLevel = 1;
    }

    public bool CheckIfICanLevelup(ref int currentLevel, LevelStorageScriptable LST)
    {
        //TODO: Add level cap
        if (LST.Levels.Length < currentLevel + 1) return false;

        if (_mM.CheckMoney(LST.Levels[currentLevel].Cost))
        {
            return true;
        }
        else return false;

    }
    public void Levelup(ref int currentLevel,LevelStorageScriptable LST,UnityEvent correspondingEvent)
    {
            _mM.DeduceMoney(LST.Levels[currentLevel].Cost);
            currentLevel++;
            correspondingEvent.Invoke();
        
    }

    private void GetData()
    {
        MapLevel = _dM.DataContainer.MapLevel;
        BoatLevel = _dM.DataContainer.BoatLevel;
        NPCFishermanLevel = _dM.DataContainer.NPCFishermanLevel;
        FishingRodLevel = _dM.DataContainer.FishingRodLevel;
        FishingHookLevel = _dM.DataContainer.FishingHookLevel;
        Multiplier = _dM.DataContainer.Multiplier;
        BoatStorageLevel = _dM.DataContainer.BoatStorageLevel;
    }

    private void SaveData()
    {
        _dM.DataContainer.MapLevel = MapLevel;
        _dM.DataContainer.BoatLevel = BoatLevel;
        _dM.DataContainer.NPCFishermanLevel = NPCFishermanLevel;
        _dM.DataContainer.FishingRodLevel = FishingRodLevel;
        _dM.DataContainer.FishingHookLevel = FishingHookLevel;
        _dM.DataContainer.Multiplier = Multiplier;
        _dM.DataContainer.BoatStorageLevel = BoatStorageLevel;
    }

    
}
