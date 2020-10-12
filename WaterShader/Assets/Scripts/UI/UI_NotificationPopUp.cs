using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NotificationPopUp : MonoBehaviour
{


    [SerializeField] private bool _isActive;



    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null) _anim = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _anim.SetBool("isActive", _isActive);

    }


}
