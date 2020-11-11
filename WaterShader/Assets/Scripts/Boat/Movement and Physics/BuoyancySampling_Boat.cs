using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancySampling_Boat : BuoyancySampling
{
    // private Boat myBoat;
    private BoatScriptable _boatSO;

    [SerializeField] private BoatScriptable debugSO;


    public override void OnInitialize()
    {
        base.OnInitialize();

        // Store Boat Info, override drag infos
        _boatSO = GetComponentInParent<BoatBase>().BoatSO;
#if UNITY_EDITOR
        _boatSO = debugSO;
#endif
        waterDrag = _boatSO.waterDrag;
        waterAngularDrag = _boatSO.waterAngularDrag;


    }
}
