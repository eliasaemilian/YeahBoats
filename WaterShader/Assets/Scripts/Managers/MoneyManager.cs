using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public float Money;

    public int Multiplyer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TMPSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
