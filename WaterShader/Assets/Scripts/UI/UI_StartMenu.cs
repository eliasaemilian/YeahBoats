using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartMenu : MonoBehaviour
{
    [SerializeField] private UI_Clouds _clouds = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartButton()
    {
        StartCoroutine("SceneLoadCoroutine");
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
}
