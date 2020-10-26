using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PondDebugButtons : MonoBehaviour
{
    [SerializeField] private int _portSceneIndex = 0;

    public void OnClickReturnToPortButton()
    {
        Debug.Log("Clicking");
        LevelManager.Instance.SaveData();
        MoneyManager.Instance.SaveData();
        Savedata.Instance.Saving();
        SceneManager.LoadScene(_portSceneIndex);
    }
}
