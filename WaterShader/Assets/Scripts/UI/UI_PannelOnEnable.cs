using Shapes;
using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class UI_PannelOnEnable : MonoBehaviour
{
    [SerializeField] private UpgradeType UT = new UpgradeType();
    [SerializeField] private ButtonType BT = new ButtonType();
    [Header("Only for boat")]
    [SerializeField] private int _boatLevel = 0;
    [SerializeField] private LevelManager LM = null;
    [SerializeField] private MoneyManager MM = null;
    private Shape _shape;
    public TextMeshProUGUI UpgradeCost = null;
    public TextMeshProUGUI UpgradeDescription = null;
    [Header("only for Map Pieces")]
    public TextMeshProUGUI MapText = null;
    public int MapLevel = 0;
    private int _upgradeCost;
    void OnEnable()
    {
        _shape = GetComponent<Shape>();

        switch (BT)
        {
            case ButtonType.Upgrade:
                SetUpgradeCost();
                SetColor();
                break;
            case ButtonType.MapDisplay:
                SetMapPieces();
                break;
            case ButtonType.MapTravel:
                UnlockThisMapGodDammit();
                break;
        }
        
    }

    public void SetUpgradeCost()
    {
        switch (UT)
        {
            case UpgradeType.BoatStorage:
                if(LM.BoatStorageLevel < LM.IndependentBoatSkillLevelCosts.BoatStorageCost.Count)
                {
                    UpgradeCost.text = "Upgrade for : " + LM.IndependentBoatSkillLevelCosts.BoatStorageCost[LM.BoatStorageLevel].ToString();
                    _upgradeCost = LM.IndependentBoatSkillLevelCosts.BoatStorageCost[LM.BoatStorageLevel];
                    SetDescription(LM.BoatStorageLevel);
                }
                else
                {
                    UpgradeCost.text = "Max Level";
                    _upgradeCost = -1;
                    SetMaxLevelDescription(LM.BoatStorageLevel);
                }
                break;
            case UpgradeType.Fisherman:
                if(LM.OwnedFishermen < LM.IndependentBoatSkillLevelCosts.FishermanCost.Count)
                {
                    UpgradeCost.text = "Buy drink for : " + LM.IndependentBoatSkillLevelCosts.FishermanCost[LM.OwnedFishermen].ToString();
                    _upgradeCost = LM.IndependentBoatSkillLevelCosts.FishermanCost[LM.OwnedFishermen];
                    SetFishermanDescription(LM.OwnedFishermen);
                }
                else
                {
                    UpgradeCost.text = "Max Level";
                    _upgradeCost = -1;
                    SetMaxLevelDescription(LM.OwnedFishermen);
                }
                break;
            case UpgradeType.FishingHook:
                if (LM.FishingHookLevel < LM.BoatSkillLevelCosts.FishingHookCost.Count)
                {
                    UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishingHookCost[LM.FishingHookLevel].ToString();
                    _upgradeCost = LM.BoatSkillLevelCosts.FishingHookCost[LM.FishingHookLevel];
                    SetDescription(LM.FishingHookLevel);
                }
                else
                {
                    UpgradeCost.text = "Max Level";
                    _upgradeCost = -1;
                    SetMaxLevelDescription(LM.FishingHookLevel);
                }
                break;
            case UpgradeType.FishingRod:
                if (LM.FishingRodLevel < LM.BoatSkillLevelCosts.FishingRodCost.Count)
                {
                    UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishingRodCost[LM.FishingRodLevel].ToString();
                    _upgradeCost = LM.BoatSkillLevelCosts.FishingRodCost[LM.FishingRodLevel];
                    SetDescription(LM.FishingRodLevel);
                }
                else
                {
                    UpgradeCost.text = "Max Level";
                    _upgradeCost = -1;
                    SetMaxLevelDescription(LM.FishingRodLevel);
                }
                break;
            case UpgradeType.FishingSpeed:
                UpgradeCost.text = "Upgrade for : " + LM.BoatSkillLevelCosts.FishingSpeedCost[LM.NPCFishermanLevel].ToString();
                _upgradeCost = LM.BoatSkillLevelCosts.FishingSpeedCost[LM.NPCFishermanLevel];
                SetDescription(LM.NPCFishermanLevel);
                break;
            case UpgradeType.TapCost:
                if(LM.TapCoinLevel < LM.IndependentBoatSkillLevelCosts.TapCoinCost.Count)
                {
                    UpgradeCost.text = "Upgrade for : " + LM.IndependentBoatSkillLevelCosts.TapCoinCost[LM.TapCoinLevel].ToString();
                    _upgradeCost = LM.IndependentBoatSkillLevelCosts.TapCoinCost[LM.TapCoinLevel];
                    SetDescription(LM.TapCoinLevel);
                }
                else
                {
                    UpgradeCost.text = "Max Level";
                    _upgradeCost = -1;
                    SetMaxLevelDescription(LM.TapCoinLevel);
                }
                break;
            case UpgradeType.Tapfish:
                if(LM.TapFishLevel < LM.IndependentBoatSkillLevelCosts.TapFishCost.Count)
                {
                    UpgradeCost.text = "Upgrade for : " + LM.IndependentBoatSkillLevelCosts.TapFishCost[LM.TapFishLevel].ToString();
                    _upgradeCost = LM.IndependentBoatSkillLevelCosts.TapFishCost[LM.TapFishLevel];
                    SetDescription(LM.TapFishLevel);
                }
                else
                {
                    UpgradeCost.text = "Max Level";
                    _upgradeCost = -1;
                    SetMaxLevelDescription(LM.TapFishLevel);
                }
                break;
            case UpgradeType.Boat:
                if (_boatLevel == LM.CurrentBoatLevel)
                {
                    UpgradeCost.text = "Current Boat";
                }
                else if (_boatLevel != LM.CurrentBoatLevel && LM.MaxBoatLevel >= _boatLevel)
                {
                    UpgradeCost.text = "Select this boat";
                }
                else
                {
                    UpgradeCost.text = "Buy for : " + LM.BoatLevels.Levels[_boatLevel - 1].Cost.ToString();
                    _upgradeCost = LM.BoatLevels.Levels[_boatLevel - 1].Cost;
                }

                    break;
            default:
                UpgradeCost.text = "no cost found";
                break;
        }

    }

    public void SetMapPieces()
    {
        if(LM.MaxMapLevel == 4)
        {
            MapText.text = "You have unlocked every Region!";
            return;
        }

        if(LM.MapPieces != 2 + 2 * LM.MaxMapLevel)
        {
            MapText.text = LM.MapPieces + " / "+(2 + 2* LM.MaxMapLevel)+" Collected";
        }
        else
        {
            MapText.text = LM.MapPieces +" / "+ (2 + 2 * LM.MaxMapLevel) + " Collected \n\r You can unlock a new Region!";
        }
    }

    public void SetColor()
    {
        if (_shape != null)
        {

            if (UT != UpgradeType.Boat)
            {
                if(_upgradeCost == -1)
                {
                    _shape.settings.fillColor = new Color(0.5f, 0.5f, 0.5f, 1);
                    return;
                }
                if (_upgradeCost < MM.Money)
                {
                    _shape.settings.fillColor = new Color(0.9245283f, 0.2791029f, 0.2791029f, 1);
                }
                else
                {
                    _shape.settings.fillColor = new Color(0.5f, 0.5f, 0.5f, 1);
                }
            }
            else
            {
                if(_boatLevel <= LM.MaxBoatLevel)
                {
                    _shape.settings.fillColor = new Color(0.9245283f, 0.2791029f, 0.2791029f, 1);

                }
                else
                {
                    if (_upgradeCost < MM.Money)
                    {
                        _shape.settings.fillColor = new Color(0.9245283f, 0.2791029f, 0.2791029f, 1);
                    }
                    else
                    {
                        _shape.settings.fillColor = new Color(0.5f, 0.5f, 0.5f, 1);
                    }
                }
            }
        }
    }

    private void SetDescription(int level)
    {

        UpgradeDescription.text = "Level " + level + " >> Level " + (level + 1);
    }
    private void SetMaxLevelDescription( int level)
    {
        UpgradeDescription.text = "Level " + level + "  Max Level Reached!";

    }
    private void SetFishermanDescription(int level)
    {

        UpgradeDescription.text = level+" Fisherman >> " + (level + 1)+ " Fisherman";
    }

    public void UpdateBoatTab()
    {
        if (_boatLevel == LM.CurrentBoatLevel)
        {
            UpgradeCost.text = "Current Boat";
        }
        else if (_boatLevel != LM.CurrentBoatLevel && LM.MaxBoatLevel >= _boatLevel)
        {
            UpgradeCost.text = "Select this boat";
        }
        else
        {
            UpgradeCost.text = "Buy for : " + LM.BoatLevels.Levels[_boatLevel - 1].Cost.ToString();
            _upgradeCost = LM.BoatLevels.Levels[_boatLevel - 1].Cost;
        }
    }

    public void UnlockThisMapGodDammit()
    {
        if(MapLevel <= LM.MaxMapLevel)
        {
            MapText.text = "Travel";

        }
        else if(MapLevel == LM.MaxMapLevel + 1 && LM.MapPieces == 2 + 2 * LM.MaxMapLevel)
        {
            MapText.text = "Unlock";
        }
        else
        {
            MapText.text = "Locked";

        }
    }
}
