using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartMenu : MonoBehaviour
{
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
        SceneManager.LoadScene(GetSceneIndex());
    }

    private int GetSceneIndex()
    {
        if (PlayerPrefs.HasKey("SceneIndex") && PlayerPrefs.GetInt("SceneIndex") > 0 && SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt("SceneIndex")) != null)
        {
            return PlayerPrefs.GetInt("SceneIndex");
        }
        else return 2;
    }
}
