using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancySampling_Boat : BuoyancySampling
{
    // private Boat myBoat;
    private BoatPhysicsSO _boatSO;


    public override void OnInitialize()
    {
        base.OnInitialize();

        // Store Boat Info, override drag infos
        _boatSO = GetComponentInParent<BoatBase>().PhysicsSO;
        waterDrag = _boatSO.waterDrag;
        waterAngularDrag = _boatSO.waterAngularDrag;


    }
}
