using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancySampling_Boat : BuoyancySampling
{
    // private Boat myBoat;
    private BoatScriptable _boatSO;



    public override void OnInitialize()
    {
        base.OnInitialize();

        // Store Boat Info
        _boatSO = GetComponentInParent<BoatBase>().BoatSO;
        waterDrag = _boatSO.waterDrag;
        waterAngularDrag = _boatSO.waterAngularDrag;
    }
}
