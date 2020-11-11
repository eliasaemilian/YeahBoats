using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoatStorageManager : MonoBehaviour
{
    [SerializeField] private LevelManager _lM = null;
    [SerializeField] private GameObject _aFKPopup = null;
    [SerializeField] private TextMeshProUGUI _aFKPopupText = null;
    private DateTime _dateTime;
    public int BoatStorage;
    public int FishInStorage;
    private DateTime currentDate;


    private void Start()
    {
        //For testing 

        PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());

        //GetData();
        //checkAfkTime();
        CalculateBoatStorage();
        InvokeRepeating("checkAfkTime", 0, 5);
    }


    private void CalculateBoatStorage()
    {
        BoatStorage = 100 * (_lM.BoatStorageLevel + 2 * _lM.CurrentBoatLevel);
    }


    private void checkAfkTime()
    {
        currentDate = System.DateTime.Now;
        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));
        DateTime oldDate = DateTime.FromBinary(temp);

        TimeSpan difference = currentDate.Subtract(oldDate);
   //     Debug.Log("Difference: " + difference);

        if(difference.TotalSeconds > 10)
        {
            AddFishToStorage(difference.TotalSeconds);
            _aFKPopup.SetActive(true);
            _aFKPopupText.text = "Welcome Back! The fishermen caught " + FishInStorage +
                " fish during your absence! You earned " + FishInStorage* FishManager.Instance._fishValue + " Coins during this time!"  ;
            MoneyManager.Instance.AddMoney(FishInStorage * FishManager.Instance._fishValue);
            
        }
            PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());
    }

    
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());

            print("Saving this date to prefs: " + System.DateTime.Now);
        }
    }

    private void AddFishToStorage(double time)
    {
        Debug.Log("Time passed : " + time);
        int fish = (int) (time / 20);

        Debug.Log("Ammount of fish : " + fish);
        if(fish > BoatStorage)
        {
            fish = BoatStorage;
        }

        FishInStorage = fish;


    }
}
