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

    private MoneyManager _mM;
    public int MapLevel;
    public int BoatLevel;
    public int NPCFishermanLevel;
    public int FishingRodLevel;
    public int Multiplyer;

    public LevelStorageScriptable NPCLevels;

    public UnityEvent NPCUpdate;
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
        _mM = MoneyManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CheckForNPCLevelup(NPCFishermanLevel + 1);
        }
    }

    private void TMPLevelSetup()
    {
        MapLevel = 1;
        BoatLevel = 1;
        NPCFishermanLevel = 1;
        FishingRodLevel = 1;
        Multiplyer = 1;
    }

    public void CheckForNPCLevelup(int nextLevel)
    {
        if (NPCLevels.Levels.Length < nextLevel) return;

        if(_mM.CheckMoney(NPCLevels.Levels[nextLevel-1].Cost))
        {
            Debug.Log("I have enough money");

            _mM.DeduceMoney(NPCLevels.Levels[nextLevel - 1].Cost);
            NPCFishermanLevel++;
            NPCUpdate.Invoke();
        }
        else
        {
            Debug.Log("I don't have enough money");

        }
    }
}
