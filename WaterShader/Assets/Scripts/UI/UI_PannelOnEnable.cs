﻿using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PannelOnEnable : MonoBehaviour
{
    [SerializeField] private UpgradeType UT = new UpgradeType();
    [SerializeField] private LevelManager LM = null;
    [SerializeField] private MoneyManager MM = null;
    private Shape _shape;
    public TextMeshProUGUI UpgradeCost = null;
    private int _upgradeCost;
    void OnEnable()
    {
        _shape = GetComponent<Shape>();
        SetUpgradeCost();
        SetColor();
    }

    public void SetUpgradeCost()
    {
        switch (UT)
        {
            case UpgradeType.BoatStorage:
                UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.BoatStorageCost[LM.BoatStorageLevel].ToString();
                _upgradeCost = LM.BoatSkillLevelCosts.BoatStorageCost[LM.BoatStorageLevel];
                break;
            case UpgradeType.Fisherman:
                UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishermanCost[LM.OwnedFishermen].ToString();
                _upgradeCost = LM.BoatSkillLevelCosts.FishermanCost[LM.OwnedFishermen];
                break;
            case UpgradeType.FishingHook:
                UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishingHookCost[LM.FishingHookLevel].ToString();
                _upgradeCost = LM.BoatSkillLevelCosts.FishingHookCost[LM.FishingHookLevel];
                break;
            case UpgradeType.FishingRod:
                UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishingRodCost[LM.FishingRodLevel].ToString();
                _upgradeCost = LM.BoatSkillLevelCosts.FishingRodCost[LM.FishingRodLevel];
                break;
            case UpgradeType.FishingSpeed:
                UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishingSpeedCost[LM.NPCFishermanLevel].ToString();
                _upgradeCost = LM.BoatSkillLevelCosts.FishingSpeedCost[LM.NPCFishermanLevel];
                break;
            default:
                UpgradeCost.text = "no cost found";
                break;
        }

    }

    public void SetColor()
    {
        if(_upgradeCost < MM.Money)
        {
             _shape.settings.fillColor = new Color(0.9245283f, 0.2791029f, 0.2791029f, 1);
        }
        else
        {
            _shape.settings.fillColor = new Color(0.5f, 0.5f, 0.5f, 1);

        }
    }

}