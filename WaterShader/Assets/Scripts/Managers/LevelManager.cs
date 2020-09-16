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
    private GameManager _gM;

    private MoneyManager _mM;
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
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }

        TMPLevelSetup();
    }
    // Start is called before the first frame update
    void Start()
    {
        _gM = GameManager.Instance;
        _mM = MoneyManager.Instance;

        //For temporary saving
        GetData();
        InvokeRepeating("SaveData", 5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //MVP
        if (Input.GetKeyDown(KeyCode.Q))
        {
           // CheckForNPCLevelup(NPCFishermanLevel + 1);
            CheckForLevelup(ref NPCFishermanLevel, NPCLevels, NPCUpdate);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //CheckForBoatStorageLevelup(BoatStorageLevel + 1);
            CheckForLevelup(ref BoatStorageLevel, BoatStorageLevels, BoatStorageUpdate);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForLevelup(ref FishingHookLevel, FishingHookLevels, FishingHookUpdate);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckForLevelup(ref FishingRodLevel, FishingRodLevels, FishingRodUpdate);
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
    //less repetitive version testing
    // TODO: write it in 2 methods for checking & actual upgrading
    public void CheckForLevelup(ref int currentLevel,LevelStorageScriptable LST,UnityEvent correspondingEvent)
    {
        if (LST.Levels.Length < currentLevel+1) return;

        if (_mM.CheckMoney(LST.Levels[currentLevel].Cost))
        {
            // UI Action if there is enough money
            Debug.Log("I have enough money");

            _mM.DeduceMoney(LST.Levels[currentLevel].Cost);
            currentLevel++;
            correspondingEvent.Invoke();
        }
        else
        {
            //UI action if ther is not enough money
            Debug.Log("I don't have enough money");

        }
    }

    private void GetData()
    {
        MapLevel = _gM.DataContainer.MapLevel;
        BoatLevel = _gM.DataContainer.BoatLevel;
        NPCFishermanLevel = _gM.DataContainer.NPCFishermanLevel;
        FishingRodLevel = _gM.DataContainer.FishingRodLevel;
        FishingHookLevel = _gM.DataContainer.FishingHookLevel;
        Multiplier = _gM.DataContainer.Multiplier;
        BoatStorageLevel = _gM.DataContainer.BoatStorageLevel;
    }

    private void SaveData()
    {
        _gM.DataContainer.MapLevel = MapLevel;
        _gM.DataContainer.BoatLevel = BoatLevel;
        _gM.DataContainer.NPCFishermanLevel = NPCFishermanLevel;
        _gM.DataContainer.FishingRodLevel = FishingRodLevel;
        _gM.DataContainer.FishingHookLevel = FishingHookLevel;
        _gM.DataContainer.Multiplier = Multiplier;
        _gM.DataContainer.BoatStorageLevel = BoatStorageLevel;
    }

    // ------ Deprecated ------
    //Call the next function to level up various stuff, not final, WIP
    //public void CheckForNPCLevelup(int nextLevel)
    //{
    //    if (NPCLevels.Levels.Length < nextLevel) return;

    //    if(_mM.CheckMoney(NPCLevels.Levels[nextLevel-1].Cost))
    //    {
    //        Debug.Log("I have enough money");

    //        _mM.DeduceMoney(NPCLevels.Levels[nextLevel - 1].Cost);
    //        NPCFishermanLevel++;
    //        NPCUpdate.Invoke();
    //    }
    //    else
    //    {
    //        Debug.Log("I don't have enough money");

    //    }
    //}
    //public void CheckForBoatStorageLevelup(int nextLevel)
    //{
    //    if (BoatStorageLevels.Levels.Length < nextLevel) return;

    //    if (_mM.CheckMoney(BoatStorageLevels.Levels[nextLevel - 1].Cost))
    //    {
    //        Debug.Log("I have enough money");

    //        _mM.DeduceMoney(BoatStorageLevels.Levels[nextLevel - 1].Cost);
    //        BoatStorageLevel++;
    //        BoatStorageUpdate.Invoke();
    //    }
    //    else
    //    {
    //        Debug.Log("I don't have enough money");

    //    }
    //}
    //public void CheckForFishingRodLevelup(int nextLevel)
    //{
    //    if (FishingRodLevels.Levels.Length < nextLevel) return;

    //    if (_mM.CheckMoney(FishingRodLevels.Levels[nextLevel - 1].Cost))
    //    {
    //        Debug.Log("I have enough money");

    //        _mM.DeduceMoney(FishingRodLevels.Levels[nextLevel - 1].Cost);
    //        FishingRodLevel++;
    //        FishingRodUpdate.Invoke();
    //    }
    //    else
    //    {
    //        Debug.Log("I don't have enough money");

    //    }
    //}
    //public void CheckForFishingHookLevelup(int nextLevel)
    //{
    //    if (FishingHookLevels.Levels.Length < nextLevel) return;

    //    if (_mM.CheckMoney(FishingHookLevels.Levels[nextLevel - 1].Cost))
    //    {
    //        Debug.Log("I have enough money");

    //        _mM.DeduceMoney(FishingHookLevels.Levels[nextLevel - 1].Cost);
    //        FishingHookLevel++;
    //        FishingHookUpdate.Invoke();
    //    }
    //    else
    //    {
    //        Debug.Log("I don't have enough money");

    //    }
    //}
}
