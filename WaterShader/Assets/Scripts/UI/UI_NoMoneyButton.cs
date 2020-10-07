using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NoMoneyButton : MonoBehaviour
{
    [SerializeField] private GameObject _tab;
    [SerializeField] private GameObject _text;

    public void DisableTab()
    {
        _tab.SetActive(false);
        _text.SetActive(false);
    }
}
