using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Clouds : MonoBehaviour
{
    [SerializeField] private GameObject _leftCloud;
    [SerializeField] private GameObject _rightCloud;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        OpenClouds();
    }
    public void OpenClouds()
    {
        _animator.SetTrigger("Open");
    }
    public void CloseClouds()
    {
        _animator.SetTrigger("Close") ;

    }
}
