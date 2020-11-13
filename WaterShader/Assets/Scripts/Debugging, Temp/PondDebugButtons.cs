using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PondDebugButtons : MonoBehaviour
{
    [SerializeField] private int _portSceneIndex = 0;
    [SerializeField] private UI_Clouds _clouds = null;

    public void OnClickReturnToPortButton()
    {
        StartCoroutine("SceneChangeCoroutine");
    }

    private IEnumerator SceneChangeCoroutine()
    {
        LevelManager.Instance.SaveData();
        MoneyManager.Instance.SaveData();
        Savedata.Instance.Saving();
        _clouds.CloseClouds();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(_portSceneIndex);
    }
}
