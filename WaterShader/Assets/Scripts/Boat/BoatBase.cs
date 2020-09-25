using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Sets up the boat and its components
/// </summary>
public class BoatBase : MonoBehaviour
{
    [SerializeField] private BoatScriptable _boatScriptable = null;
    [SerializeField] private NPCSpotsScript _nPCSpots = null;
    [SerializeField] private GameObject _nPCFishermanPrefab = null;
    [SerializeField] private LevelStorageScriptable _boatLevels = null;

    private LevelManager _lM;

    public List<GameObject> SpawnPoints = new List<GameObject>();

    public UnityEvent FishingSpeedup;

    public int BoatStorage;
    private int _currentBoatStorage;

    // Start is called before the first frame update
    void Start()
    {
        _lM = LevelManager.Instance;
        InstantiateBoat();

        BoatStorageUpdate();
        //_lM.BoatStorageUpdate.AddListener(BoatStorageUpdate);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(10, 90, 150, 20), "Add Fisherman"))
        {
            AddFisherman();
        }
        if(GUI.Button(new Rect(10, 110, 150, 20), "Speed up Catch"))
        {
            TapToSpeedupCatch();
        }
    }

    private void InstantiateBoat()
    {
        GameObject b = _boatLevels.Levels[_lM.BoatLevel - 1].BoatPrefab;
        GameObject boat = Instantiate(b, transform.position, Quaternion.identity);
        boat.transform.parent = this.transform;
        _nPCSpots = boat.gameObject.GetComponentInChildren<NPCSpotsScript>();
        Debug.Log("Loading boat of level : " + _boatLevels.Levels[_lM.BoatLevel - 1].Level);
    }
    private bool AddFisherman()
    {
        if (SpawnPoints.Count < _boatScriptable.NPCSpawnPointAmmount)
        {
            GameObject gO = _nPCSpots._spots[SpawnPoints.Count];
            SpawnPoints.Add(gO);

            GameObject fisherman = Instantiate(_nPCFishermanPrefab,gO.transform.position,gO.transform.rotation, gO.transform);
            fisherman.GetComponent<NPC_Fisherman>().BB = this;
            return true;
        }
        else return false;
    }


    public bool AddFishToStorage()
    {
        if(_currentBoatStorage < BoatStorage)
        {
            _currentBoatStorage++;
            return true;
        }
        else
        {
           return false;
        }
    }

    public void TapToSpeedupCatch()
    {
        FishingSpeedup.Invoke();
    }
    public void EmptyStorage()
    {
        _currentBoatStorage = 0;
    }

    private void BoatStorageUpdate()
    {
        BoatStorage = 10 * (2 * _lM.BoatStorageLevel);
    }
}
