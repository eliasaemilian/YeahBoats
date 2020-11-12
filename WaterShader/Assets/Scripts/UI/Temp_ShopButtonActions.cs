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

    private bool _shootFireworks = false;
    void Start()
    {
        foreach(VisualEffect firework in _FireworkPrefabs)
        {
            firework.Stop();
        }
    }

    public void OnClickLighthouseVoyageButton(int Sceneindex)
    {
        if(Sceneindex - 1 <= LM.MaxMapLevel)
        {
            if(Sceneindex - 1 <= 2 && LM.HighSeaboat())
            {
                _changeBoatPannel.SetActive(true);
            }
            else
            {


            // go to this map
            LevelManager.Instance.SaveData();
            MoneyManager.Instance.SaveData();
            Savedata.Instance.Saving();
            SceneManager.LoadScene(Sceneindex);
            }
        }
        else
        {
            if(LM.MapPieces == 4)
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
        if (LM.CheckIfICanLevelup(LM.FishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost))
        {
            //TODO: Particle effect
            Debug.Log("I can Level Up");
            LM.FishingHookLevel = LM.Levelup(LM.FishingHookLevel, LM.BoatSkillLevelCosts.FishingHookCost);
            ResetColors();
            _shootFireworks = true;
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
            LM.FishingRodLevel = LM.Levelup(LM.FishingRodLevel, LM.BoatSkillLevelCosts.FishingRodCost);
            ResetColors();
            _shootFireworks = true;
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);

        }
    }
    public void OnClickUpgradeTapCoinButton()
    {
        if (LM.CheckIfICanLevelup(LM.TapCoinLevel, LM.IndependentBoatSkillLevelCosts.TapCoinCost))
        {
            LM.TapCoinLevel = LM.Levelup(LM.TapCoinLevel, LM.IndependentBoatSkillLevelCosts.TapCoinCost);
            ResetColors();
            _shootFireworks = true;
        }
        else
        {
            Debug.Log("I can not Level Up");
            _notEnoughMoneyPannel.SetActive(true);
        }
    }
    public void OnClickUpgradeTapFishButton()
    {
        if (LM.CheckIfICanLevelup(LM.TapFishLevel, LM.IndependentBoatSkillLevelCosts.TapFishCost))
        {
            LM.TapFishLevel = LM.Levelup(LM.TapFishLevel, LM.IndependentBoatSkillLevelCosts.TapFishCost);
            ResetColors();
            _shootFireworks = true;
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
                LM.OwnedFishermen = LM.Levelup(LM.OwnedFishermen, LM.BoatSkillLevelCosts.FishermanCost);
                ResetColors();
                ShootFireworks();
            }
            else
            {
                //Hire failed
                LM.Levelup(LM.OwnedFishermen, LM.BoatSkillLevelCosts.FishermanCost);
                ResetColors();
                _fishermanFailHirePannel.SetActive(true);
            }
        }
        else
        {
            _notEnoughMoneyPannel.SetActive(true);
        }
    }

    public void OnClickBoatButton(int ButtonLevel)
    {
        if(LM.MaxBoatLevel == ButtonLevel - 1)
        {
            if (LM.CheckIfICanLevelupBoat(LM.MaxBoatLevel))
            {
                LM.MaxBoatLevel = LM.LevelupBoat(LM.MaxBoatLevel);
                ResetColors();
                _shootFireworks = true;
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

}
