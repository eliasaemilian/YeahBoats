using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuDebugButtons : MonoBehaviour
{

    [SerializeField] private int _fishingSceneIndex;
    [SerializeField] private int _portSceneIndex;


    public void OnClickFishing()
    {
        SceneManager.LoadScene(_fishingSceneIndex);
    }

    public void OnClickPort()
    {
        SceneManager.LoadScene(_portSceneIndex);
    }
}
