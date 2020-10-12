using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NotificationPopUp : MonoBehaviour
{

    [SerializeField ]Vector3 moveUpwards, moveDownwards;
    [SerializeField] private float _speed = 1f;

    [SerializeField] private bool _isActive;

    private bool startActivation;

    [SerializeField] private Camera _uiCamera;

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null) _anim = GetComponentInChildren<Animator>();
        
        GetMovementInRelationToScreensize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _anim.SetBool("isActive", _isActive);


        if (_isActive)
        {
            //  StartCoroutine(PopUpActive());
            
         //   PopUp();
        }
        else StopAllCoroutines();
    }

    float t = 0;
    bool goDown;
    private void  PopUp()
    {
        // _disc.
        if (t > 1) goDown = true;
        if (t < -1) goDown = false;

        Vector3 pos;
        if (goDown)
        {
            t -= Time.deltaTime;
            pos = moveDownwards;
        }
        else
        {
            t += Time.deltaTime;
            pos = moveUpwards;
        }

       transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * _speed);
      //  transform.position = new Vector3(transform.position.x, (moveUpwards.y * t), transform.position.z);
    }

    private void GetMovementInRelationToScreensize()
    {
        float camY = _uiCamera.orthographicSize * 2;

        float offset = ( Screen.safeArea.height / camY );
        offset *= 0.005f;
        Debug.Log("Offset " + offset);

        moveDownwards = new Vector3 (transform.position.x, transform.position.y - offset, transform.position.z);
        moveUpwards = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
    }
}
