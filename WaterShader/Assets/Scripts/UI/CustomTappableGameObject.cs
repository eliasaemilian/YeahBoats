using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TabType
{
    Menu,
    Notification
}
public class CustomTappableGameObject :TappableGameobject
{
    [SerializeField] private UpgradeType UT = new UpgradeType();
    [SerializeField] private TabType TT = new TabType();
    [SerializeField] private Temp_ShopButtonActions _TSA = null;
    [SerializeField] private UI_NoMoneyButton _UINMB = null;
    private UI_PannelOnEnable _UIPO = null;

    void Start()
    {
        _UIPO = GetComponent<UI_PannelOnEnable>();
    }

    //2D UI
    public override void OnTap(Touch touch, Vector3 pos)
    {
        if(TT == TabType.Menu)
        {

        base.OnTap(touch, pos);
        Debug.Log("Tapping On Button");
        Upgrade();
            if(_UIPO != null)
            {
                _UIPO.SetColor();
                _UIPO.SetUpgradeCost();
            }
        }
        else
        {
            _UINMB.DisableTab();
        }
    }

    private void Upgrade()
    {
        switch (UT)
        {
            case UpgradeType.BoatStorage:
                break;
            case UpgradeType.Fisherman:
                _TSA.OnClickPubHireMenButton();
                break;
            case UpgradeType.FishingHook:
                _TSA.OnClickUpgradeFishingHookButton();
                break;
            case UpgradeType.FishingRod:
                _TSA.OnClickUpgradeFishingRodButton();
                break;
            case UpgradeType.FishingSpeed:
                break;
            case UpgradeType.TapCost:
                break;
            case UpgradeType.Tapfish:
                break;
            default:
                break;
        }
    }
}
