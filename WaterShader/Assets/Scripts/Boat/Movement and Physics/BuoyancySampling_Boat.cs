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

        // Store Boat Info, override drag infos
        if (GetComponentInParent<BoatBase>() != null) _boatSO = GetComponentInParent<BoatBase>().BoatSO;
        else _boatSO = LevelManager.Instance.GetCurretBoatPhysicsSO();
        waterDrag = _boatSO.waterDrag;
        waterAngularDrag = _boatSO.waterAngularDrag;


    }
}
