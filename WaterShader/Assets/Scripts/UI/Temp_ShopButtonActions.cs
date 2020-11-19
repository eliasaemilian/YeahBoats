using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public enum UpgradeType
{
    BoatStorage,
    Fisherman,
    FishingHook,
    FishingRod,
    FishingSpeed,
    TapCost,
    Tapfish,
    Boat
}
public enum ButtonType
{
    Upgrade,
    MapDisplay,
    MapTravel
}
public enum MapLevel
{
    Port,
    Pond,
    Desert,
    Mountains,
    Ocean
}
public class Temp_ShopButtonActions : MonoBehaviour
{
    [SerializeField] private int _sceneIndexPond = 0;
    [SerializeField] private LevelManager LM = null;
    [SerializeField] private List<UI_PannelOnEnable> _pannels = null;
    [SerializeField] private List<UI_PannelOnEnable> _boatPannels = null;
    [SerializeField] private List<UI_PannelOnEnable> _lighthousePannels = null;
    [SerializeField] private GameObject _notEnoughMoneyPannel = null;
    [SerializeField] private GameObject _fishermanFailHirePannel = null;
    [SerializeField] private GameObject _changeBoatPannel = null;
    [SerializeField] private List<VisualEffect> _FireworkPrefabs = null;
    [SerializeField] private UI_Clouds _clouds = null;

    private bool _shootFireworks = false;
    void Start()
    {
        foreach(VisualEffect firework in _FireworkPrefabs)
        {
            firework.Stop();
        }
    }

    public void OnClickLighthouseVoyageButton(int mapLevel)
    {
        if(mapLevel <= LM.MaxMapLevel)
        {
            if(mapLevel <= 2 && LM.HighSeaboat())
            {
                _changeBoatPannel.SetActive(true);
            }
            else
            {
                StartCoroutine("SceneChangeCoroutine", mapLevel);
            }
        }
        else
        {
            if(LM.MapPieces == 2 + 2*LM.MaxMapLevel)
            {
                LM.MapPieces = 0;
                LM.MaxMapLevel++;
                UpdateLighthousePannels();
                _shootFireworks = true;
            }
            else{
                //yade yade dont do this
            }
        }
        // Scene Switch to Pond
        //Debug.Log("Loading Pond");
    }

    public void OnClickUpgradeFishingHookButton()
    {
        if(LM.FishingHookLevel < LM.BoatSkillLevelCosts.FishingHookCost.Count)
        {
        if (LM.CheckIfICanLevelup(LM.FishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost))
        {
            Debug.Log("I can Level Up");
            LM.FishingHookLevel = LM.Levelup(LM.FishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost);
            ExecuteValidUpgradeReaction();
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);

        }
        }
    }
    public void OnClickUpgradeFishingRodButton()
    {
        if(LM.FishingRodLevel < LM.BoatSkillLevelCosts.FishingRodCost.Count)
        {

        if (LM.CheckIfICanLevelup(LM.FishingRodLevel,LM.BoatSkillLevelCosts.FishingRodCost))
        {
            LM.FishingRodLevel = LM.Levelup(LM.FishingRodLevel, LM.BoatSkillLevelCosts.FishingRodCost);
            ExecuteValidUpgradeReaction();
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);

        }
        }
    }
    public void OnClickUpgradeTapCoinButton()
    {
        if(LM.TapCoinLevel < LM.IndependentBoatSkillLevelCosts.TapCoinCost.Count)
        {

        if (LM.CheckIfICanLevelup(LM.TapCoinLevel, LM.IndependentBoatSkillLevelCosts.TapCoinCost))
        {
            LM.TapCoinLevel = LM.Levelup(LM.TapCoinLevel, LM.IndependentBoatSkillLevelCosts.TapCoinCost);
            ExecuteValidUpgradeReaction();
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);
        }
        }
    }
    public void OnClickUpgradeTapFishButton()
    {
        if (LM.TapFishLevel < LM.IndependentBoatSkillLevelCosts.TapFishCost.Count)
        {
            if (LM.CheckIfICanLevelup(LM.TapFishLevel, LM.IndependentBoatSkillLevelCosts.TapFishCost))
            {
                LM.TapFishLevel = LM.Levelup(LM.TapFishLevel, LM.IndependentBoatSkillLevelCosts.TapFishCost);
                ExecuteValidUpgradeReaction();
            }
            else
            {
                Debug.Log("I can not Level Up");
                _notEnoughMoneyPannel.SetActive(true);
            }
        }
    }
    public void OnClickPubHireMenButton()
    {
        if(LM.OwnedFishermen < LM.IndependentBoatSkillLevelCosts.FishermanCost.Count)
        {

        if (LM.CheckIfICanLevelup(LM.OwnedFishermen, LM.IndependentBoatSkillLevelCosts.FishermanCost))
        {
            if(LM.ChanceLevelupForFisherman(25))
            {
                //Hire succeeded
                LM.OwnedFishermen = LM.Levelup(LM.OwnedFishermen, LM.IndependentBoatSkillLevelCosts.FishermanCost);
                ExecuteValidUpgradeReaction();
            }
            else
            {
                //Hire failed
                LM.Levelup(LM.OwnedFishermen, LM.IndependentBoatSkillLevelCosts.FishermanCost);
                ResetColors();
                _fishermanFailHirePannel.SetActive(true);
            }
        }
        else
        {
            _notEnoughMoneyPannel.SetActive(true);
        }
        }
    }

    public void OnClickBoatButton(int ButtonLevel)
    {
        if(LM.MaxBoatLevel == ButtonLevel - 1)
        {
            if (LM.CheckIfICanLevelupBoat(LM.MaxBoatLevel))
            {
                LM.MaxBoatLevel = LM.LevelupBoat(LM.MaxBoatLevel);
                ExecuteValidUpgradeReaction();
            }
            else
            {
                Debug.Log("I can not Level Up");
                _notEnoughMoneyPannel.SetActive(true);

            }
        }
        else if (LM.CurrentBoatLevel != ButtonLevel)
        {
            LM.CurrentBoatLevel = ButtonLevel;
            LM.ReloadBoatUpgrades();
            
        }
        else
        {
            LM.CurrentBoatLevel = ButtonLevel;
            LM.ReloadBoatUpgrades();

        }

    }

    private void ExecuteValidUpgradeReaction()
    {
        ResetColors();
        _shootFireworks = true;
        SoundscapeManager.PlaySound.Invoke(0);
    }
   
    public void UpdateBoatPanels()
    {
        foreach (UI_PannelOnEnable panel in _boatPannels)
        {
            panel.UpdateBoatTab();
        }
    }
    public void UpdateLighthousePannels()
    {
        foreach (UI_PannelOnEnable panel in _lighthousePannels)
        {
            panel.UnlockThisMapGodDammit();
        }
    }
    private void ResetColors()
    {
        foreach (UI_PannelOnEnable panel in _pannels)
        {
            panel.SetColor();
        }
    }
    
    public void ShootFireworks()
    {
        if (_shootFireworks)
        {

        foreach (VisualEffect firework in _FireworkPrefabs)
        {
            firework.Play();
            firework.playRate = 2;

        }
            _shootFireworks = false;
        }
        // And sum audio to come here
    }

    private IEnumerator SceneChangeCoroutine(int Sceneindex)
    {
        LevelManager.Instance.SaveData();
        MoneyManager.Instance.SaveData();
        Savedata.Instance.Saving();
        _clouds.CloseClouds();
        yield return new WaitForSeconds(1);
        SceneChecker(Sceneindex);
    }

    private void SceneChecker(int Sceneindex)
    {
        switch ((MapLevel)Sceneindex)
        {
            case MapLevel.Port:
                SceneManager.LoadScene(1);
                break;
            case MapLevel.Pond:
                SceneManager.LoadScene(2);

                break;
            case MapLevel.Desert:
                SceneManager.LoadScene(4);

                break;
            case MapLevel.Mountains:
                SceneManager.LoadScene(3);

                break;
            case MapLevel.Ocean:
                SceneManager.LoadScene(5);

                break;
            default:
                break;
        }
    }
}
