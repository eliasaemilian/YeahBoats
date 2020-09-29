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
        SceneManager.LoadScene(_portSceneIndex);
    }
}
