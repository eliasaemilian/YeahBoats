using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NoMoneyButton : MonoBehaviour
{
    [SerializeField] private GameObject _tab;

    public void DisableTab()
    {
        _tab.SetActive(false);
    }
}
