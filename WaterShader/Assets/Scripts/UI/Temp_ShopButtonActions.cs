﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_ShopButtonActions : MonoBehaviour
{
    [SerializeField] private int _sceneIndexPond = 0;

    public void OnClickLighthouseVoyageButton()
    {
        // Scene Switch to Pond
        Debug.Log("Loading Pond");
        SceneManager.LoadScene(_sceneIndexPond);
    }

    public void OnClickShackUpgradeButton()
    {
        // Whatever Shack does
    }

    public void OnClickPubHireMenButton()
    {
        // Hire big burly seamen for ship right here
    }

    
}
