﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
/// <summary>
/// Managesh the fishes, their cost and rarity
/// </summary>
public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    public LevelManager _lM;
    public float _fishValue;
    private float _baseFishValue;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        _baseFishValue = 5;
        UpdateFishValue();
        _lM.FishingRodUpdate.AddListener(UpdateFishValue);
    }


    //does all needed to get fish
    public float GetFish()
    {
        if (CheckForRareCatch())
        {
            return _fishValue * 5;
        }
        else return _fishValue;
    }

    private void UpdateFishValue()
    {
        _fishValue = (_baseFishValue* _lM.CurrentBoatLevel + 2* _lM.FishingRodLevel) * Mathf.Sqrt(_lM.CurrentMapLevel * _lM.CurrentBoatLevel) * _lM.Multiplier* _lM.CurrentBoatLevel * _lM.CurrentMapLevel;
    }

    public bool CheckForRareCatch()
    {
        int rand = Random.Range(1, 101);

        if (rand < 1 + Mathf.Sqrt(_lM.FishingHookLevel * 4))
        {
            return true;
        }
        else return false;
    }
}
