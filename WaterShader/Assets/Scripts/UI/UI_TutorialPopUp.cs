using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialPopUp : TappableGameobject
{
    [SerializeField] private TutorialSet tutSet = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTutorialTriggered()
    {
        // get size somehow bleb
        // put text into text field
        // put at right position
        // plop up background
        // enable text
        // start arrow movement
    }

    public override void OnTap(Touch touch, Vector3 pos)
    {
        base.OnTap(touch, pos);
        // disbale tut box, move to next tut box
    }

    IEnumerator OpenTutorialPopUp()
    {
        yield return new WaitForSeconds(3f);
    }
}
