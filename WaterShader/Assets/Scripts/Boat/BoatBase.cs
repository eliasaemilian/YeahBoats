using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Sets up the boat and its components
/// </summary>
public class BoatBase : MonoBehaviour
{
    [SerializeField] private BoatScriptable debugSO;
    [SerializeField] private NPCSpotsScript _nPCSpots = null;
    [SerializeField] private GameObject _nPCFishermanPrefab = null;

    private BoatScriptable _boatScriptable = null;
    private LevelManager _lM;

    public UnityEvent FishingSpeedup;
    public BoatScriptable BoatSO { get { return _boatScriptable; } set { _boatScriptable = value; } }

    private void Awake()
    {
        _lM = LevelManager.Instance;
        BoatSO = _lM.GetCurretBoatPhysicsSO();

        #if UNITY_EDITOR
        if (BoatSO == null) BoatSO = debugSO;
        #endif
    }
    void Start()
    {
        WaterTappableHandler.FishingTap?.AddListener(FishingSpeedup.Invoke);
        _nPCSpots = GetComponentInChildren<NPCSpotsScript>();
        InstantiateFishermen();
    }


    void OnGUI()
    {
        if(GUI.Button(new Rect(10, 90, 150, 20), "Add Fisherman"))
        {
            BuyFisherman();
        }
        if(GUI.Button(new Rect(10, 110, 150, 20), "Speed up Catch"))
        {
            TapToSpeedupCatch();
        }
    }

    private void InstantiateFishermen()
    {
        int fisherManCount = Mathf.Min(_lM.OwnedFishermen, _lM.MaxAmmountOfFishermen);

        for (int i = 0; i < fisherManCount; i++)
        {
            AddFisherman(i);
        }
    }
    private void AddFisherman(int spotIndex)
    {
        if (_nPCSpots._spots.Length >= spotIndex)
        {
            GameObject gO = _nPCSpots._spots[spotIndex];
            GameObject fisherman = Instantiate(_nPCFishermanPrefab,gO.transform.position,gO.transform.rotation, gO.transform);
            fisherman.GetComponent<NPC_Fisherman>().BB = this;
        }
        else
        {
           // Debug.LogError("Not enough fishermap spots");
        }
    }

    private void BuyFisherman()
    {
        _lM.OwnedFishermen++;
    }
    public void TapToSpeedupCatch()
    {
        FishingSpeedup.Invoke();
    }
    
}
