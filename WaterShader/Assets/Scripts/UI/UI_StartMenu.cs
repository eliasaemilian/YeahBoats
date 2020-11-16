using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_StartMenu : MonoBehaviour
{
    [SerializeField] private UI_Clouds _clouds = null;
    [SerializeField] private TextMeshProUGUI _creditsTextfield = null;
    [SerializeField] private GameObject _creditsGO = null;
    [SerializeField] private GameObject _startButton = null;
    [SerializeField] private TextAsset _credtisTextAsset = null;


    // Start is called before the first frame update
    void Start()
    {
        ImportCreditsTextAsset();
        SoundscapeManager.QueueNewMusic.Invoke(0);
    }


    public void OnClickCreditsButton()
    {
        _creditsGO.SetActive(!_creditsGO.activeSelf);
        _startButton.SetActive(!_creditsGO.activeSelf);
    }

    public void OnClickStartButton()
    {
        StartCoroutine(SceneLoadCoroutine());
    }

    private int GetSceneIndex()
    {
        if (PlayerPrefs.HasKey("SceneIndex") && PlayerPrefs.GetInt("SceneIndex") > 0 && SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt("SceneIndex")) != null)
        {
            return PlayerPrefs.GetInt("SceneIndex");
        }
        else return 2;
    }
    private IEnumerator SceneLoadCoroutine()
    {
        _clouds.CloseClouds();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(GetSceneIndex());
    }

    private void ImportCreditsTextAsset()
    {
        _creditsTextfield.text = _credtisTextAsset.text;
    }
}
