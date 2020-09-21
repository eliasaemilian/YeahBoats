using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterTappable : TappableGameobject
{
    
    public override void OnTap(Touch touch, Vector3 pos, float dist)
    {
        base.OnTap(touch, pos, dist);
    }
}
