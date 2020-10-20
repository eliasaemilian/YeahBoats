using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateTappable : TappableGameobject
{
    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        
        base.OnTap(touch, pos, dist);

        Debug.Log("I got tapped");

    }
}
