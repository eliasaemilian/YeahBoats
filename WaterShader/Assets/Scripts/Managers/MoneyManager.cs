using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MoneyManager is responsible for saving and editing data related to the game currency
/// </summary>
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;
    public DataManager _dM;

    public float Money;

    public int Multiplyer;

    void Awake()
    {
         Instance = this;
     
    }
    // Start is called before the first frame update
    void Start()
    {
        //_dM = DataManager.Instance;


        TMPSetup();

        //For temporary saving
        GetData();
        InvokeRepeating("SaveData", 5, 5);

    }

    private void TMPSetup()
    {
        Money = 0;
        Multiplyer = 1;
    }

    public void AddMoney(float ammount)
    {
        Money += (ammount * Multiplyer);
    }

    public bool CheckMoney(float ammount)
    {
        return Money >= ammount? true : false;
    }

    public void DeduceMoney(float ammount)
    {
        Money -= ammount;
    }

    private void GetData()
    {
        Money = _dM.DataContainer.Money;
        Multiplyer = _dM.DataContainer.Multiplyer;
    }
    private void SaveData()
    {
        _dM.DataContainer.Money = Money;
        _dM.DataContainer.Multiplyer = Multiplyer;
    }
}
