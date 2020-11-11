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
    private BoatScriptable _boatScriptable = null;
    public BoatScriptable BoatSO { get { return _boatScriptable; } set { _boatScriptable = value; } }

    [SerializeField] private NPCSpotsScript _nPCSpots = null;
    [SerializeField] private GameObject _nPCFishermanPrefab = null;

    private LevelManager _lM;

    public List<GameObject> SpawnPoints = new List<GameObject>();

    public UnityEvent FishingSpeedup;

    private void Awake()
    {
        _lM = LevelManager.Instance;
        //Debug.Log("BoatLevels length : "+_lM.BoatLevels.Levels.Length);
        //Debug.Log("Current Boat Level : "+_lM.CurrentBoatLevel);
        //Debug.Log("BoatLevels length : "+_lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1]);
        BoatSO = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].boatScriptable;
        Debug.Log("BoatSO is " + BoatSO);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (WaterTappableHandler.FishingTap != null)
        {
            WaterTappableHandler.FishingTap.AddListener(FishingSpeedup.Invoke);
        }
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

    private void InstantiateBoat()
    {
        Debug.Log("Current Boat Level : " + _lM.CurrentBoatLevel);
        GameObject b = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].BoatPrefab;
        _lM.BoatSkillLevels = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1];
        _lM.MaxAmmountOfFishermen = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].NPCFishermanAmmount;
        GameObject boat = Instantiate(b, transform.position, Quaternion.identity);
        boat.transform.parent = this.transform;
    }
    private void InstantiateFishermen()
    {
        if(_lM.OwnedFishermen > _lM.MaxAmmountOfFishermen)
        {
            for (int i = 0; i < _lM.MaxAmmountOfFishermen; i++)
            {
                AddFisherman();
            }
        }
        else
        {
            for (int i = 0; i < _lM.OwnedFishermen; i++)
            {
                AddFisherman();
            }
        }
    }
    private bool AddFisherman()
    {
        if (SpawnPoints.Count < _lM.MaxAmmountOfFishermen)
        {
            GameObject gO = _nPCSpots._spots[SpawnPoints.Count];
            SpawnPoints.Add(gO);

            GameObject fisherman = Instantiate(_nPCFishermanPrefab,gO.transform.position,gO.transform.rotation, gO.transform);
            fisherman.GetComponent<NPC_Fisherman>().BB = this;
            
            return true;
        }
        else return false;
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
