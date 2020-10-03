using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UpgradeType
{
    BoatStorage,
    Fisherman,
    FishingHook,
    FishingRod,
    FishingSpeed
}
public class Temp_ShopButtonActions : MonoBehaviour
{
    [SerializeField] private int _sceneIndexPond = 0;
    [SerializeField] private LevelManager LM = null;
    [SerializeField] private List<UI_PannelOnEnable> _pannels = null;
    [SerializeField] private GameObject _notEnoughMoneyPannel = null;
    [SerializeField] private GameObject _fishermanFailHirePannel = null;

    public void OnClickLighthouseVoyageButton()
    {
        // Scene Switch to Pond
        Debug.Log("Loading Pond");
        SceneManager.LoadScene(_sceneIndexPond);
    }

    public void OnClickUpgradeFishingHookButton()
    {
        if (LM.CheckIfICanLevelup(LM.FishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost))
        {
            //TODO: Particle effect
            Debug.Log("I can Level Up");
            LM.FishingHookLevel = LM.Levelup(LM.FishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost, LM.FishingHookUpdate);
            ResetColors();
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);

        }
    }
    public void OnClickUpgradeFishingRodButton()
    {
        if (LM.CheckIfICanLevelup(LM.FishingRodLevel,LM.BoatSkillLevelCosts.FishingRodCost))
        {
            LM.FishingRodLevel = LM.Levelup(LM.FishingRodLevel, LM.BoatSkillLevelCosts.FishingRodCost, LM.FishingRodUpdate);
            ResetColors();
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);

        }
    }

    public void OnClickPubHireMenButton()
    {
        if (LM.CheckIfICanLevelup(LM.OwnedFishermen, LM.BoatSkillLevelCosts.FishermanCost))
        {
            if(LM.ChanceLevelupForFisherman(25))
            {
                //Hire succeeded
                LM.OwnedFishermen = LM.Levelup(LM.OwnedFishermen, LM.BoatSkillLevelCosts.FishermanCost, LM.NPCUpdate);
            }
            else
            {
                //Hire failed
                _fishermanFailHirePannel.SetActive(true);
            }
        }
        else
        {
            _notEnoughMoneyPannel.SetActive(true);
        }
    }

   
    private void ResetColors()
    {
        foreach (UI_PannelOnEnable panel in _pannels)
        {
            panel.SetColor();
        }
    }
}
