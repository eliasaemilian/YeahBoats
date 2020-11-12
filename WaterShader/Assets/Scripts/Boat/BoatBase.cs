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
    [SerializeField] private BoatPhysicsSO debugPhysics;
    [SerializeField] private NPCSpotsScript _nPCSpots = null;
    [SerializeField] private GameObject _nPCFishermanPrefab = null;

    private BoatPhysicsSO _physicsSO = null;
    private LevelManager _lM;

    public UnityEvent FishingSpeedup;
    public BoatPhysicsSO PhysicsSO { get { return _physicsSO; } set { _physicsSO = value; } }

    private void Awake()
    {
        _lM = LevelManager.Instance;
        PhysicsSO = _lM.GetCurrentBoatPhysicsSO();

        #if UNITY_EDITOR
        if (PhysicsSO == null)
            PhysicsSO = debugPhysics;
        #endif
    }

    void Start()
    {
        WaterTappableHandler.FishingTap?.AddListener(FishingSpeedup.Invoke);
        _nPCSpots = GetComponentInChildren<NPCSpotsScript>();
        InstantiateFishermen();
    }

    private void BuyFisherman()
    {
        _lM.OwnedFishermen++;
    }

    public void TapToSpeedupCatch()
    {
        FishingSpeedup.Invoke();
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
        if (_nPCSpots._spots.Length <= spotIndex)
        {
            GameObject gO = _nPCSpots._spots[spotIndex];
            GameObject fisherman = Instantiate(_nPCFishermanPrefab, gO.transform.position, gO.transform.rotation, gO.transform);
            fisherman.GetComponent<NPC_Fisherman>().BB = this;
        }
        else
        {
            Debug.LogError("Not enough fishermap spots");
        }
    }
}