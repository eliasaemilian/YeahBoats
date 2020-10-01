using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditorInternal;
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
            LM.Levelup(ref LM._fishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost, LM.FishingHookUpdate);
            ResetColors();
        }
        else
        {
            Debug.Log("I can not Level Up");

        }
    }
    public void OnClickUpgradeFishingRodButton()
    {
        if (LM.CheckIfICanLevelup(LM.FishingRodLevel,LM.BoatSkillLevelCosts.FishingRodCost))
        {
            LM.Levelup(ref LM._fishingRodLevel, LM.BoatSkillLevelCosts.FishingRodCost, LM.FishingRodUpdate);
            ResetColors();
        }
        else
        {
            Debug.Log("I can not Level Up");

        }
    }

    public void OnClickPubHireMenButton()
    {
        // Hire big burly seamen for ship right here
    }

   
    private void ResetColors()
    {
        foreach (UI_PannelOnEnable panel in _pannels)
        {
            panel.SetColor();
        }
    }
}
