using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MoneyManager is responsible for saving and editing data related to the game currency
/// </summary>
public class MoneyManager : Singleton<MoneyManager>
{
    private DataManager _dM;
    public CanvasDisplay CD;
    public float Money;

    public int Multiplyer;

    protected override void Awake()
    {
        base.Awake();
        _dM = GameObject.Find("DataManager").GetComponent<DataManager>();
        
    }
    void Start()
    {

        TMPSetup();
        LevelManager.Instance.MultiplierUpdate.AddListener(UpdateMultiplier);
        //For temporary saving
        GetData();
        InvokeRepeating("SaveData", 5, 5);
        CD.UpdateCurrency((int)Money);

    }

    private void TMPSetup()
    {
        Money = 0;
        Multiplyer = 1;
    }
    public void ResetMoney()
    {
        Money = 0;
        CD.UpdateCurrency((int)Money);
    }
    public void AddMoney(float ammount)
    {
        Money += (ammount * Multiplyer);
        CD.UpdateCurrency((int)Money);
    }

    public bool CheckMoney(float ammount)
    {
        return Money >= ammount? true : false;
    }

    public void DeduceMoney(float ammount)
    {
        Money -= ammount;
    }

    public void UpdateMoney()
    {
        CD.UpdateCurrency((int)Money);
    }
    private void UpdateMultiplier()
    {
        Multiplyer = LevelManager.Instance.Multiplier;
    }
    private void GetData()
    {
        Money = _dM.DataContainer.Money;
        Multiplyer = _dM.DataContainer.Multiplyer;
    }
    public void SaveData()
    {
        _dM.DataContainer.Money = Money;
        _dM.DataContainer.Multiplyer = Multiplyer;
    }
}
