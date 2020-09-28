﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CanvasDisplay : MonoBehaviour
{
    public static CanvasDisplay Instance;

    public TextMeshProUGUI _text;
    public TextMeshProUGUI CurrencyCounter;

    void Awake()
    {
        Instance = this;  
    }

    public void UpdateText(string res)
    {
        _text.text = res;
        
    }

    public void UpdateCurrency(int gold)
    {
        CurrencyCounter.text = gold.ToString();
    }
}