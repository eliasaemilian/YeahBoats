using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PondDebugButtons : MonoBehaviour
{
    [SerializeField] private int _portSceneIndex;

    public void OnClickReturnToPortButton()
    {
        SceneManager.LoadScene(_portSceneIndex);
    }
}
