using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

/// <summary>
/// Sets up the boat and its components
/// </summary>
public class BoatBase : MonoBehaviour
{
    [SerializeField] private BoatScriptable _boatScriptable = null;
    public BoatScriptable BoatSO { get { return _boatScriptable; } set { _boatScriptable = value; } }

    [SerializeField] private NPCSpotsScript _nPCSpots = null;
    [SerializeField] private GameObject _nPCFishermanPrefab = null;

    private LevelManager _lM = LevelManager.Instance;

    public List<GameObject> SpawnPoints = new List<GameObject>();

    public int BoatStorage;
    private int _currentBoatStorage;
    // Start is called before the first frame update
    void Start()
    {
        //AddFisherman();
        BoatStorageUpdate();
        _lM.BoatStorageUpdate.AddListener(BoatStorageUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddFisherman();
        }

        //MVP
        if (Input.GetKeyDown(KeyCode.A))
        {
            EmptyStorage();
        }

    }
    private bool AddFisherman()
    {
        if (SpawnPoints.Count < BoatSO.NPCSpawnPointAmmount)
        {
            GameObject gO = _nPCSpots._spots[SpawnPoints.Count];
            SpawnPoints.Add(gO);

            GameObject fisherman = Instantiate(_nPCFishermanPrefab,gO.transform.position,gO.transform.rotation, gO.transform);
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

    public void EmptyStorage()
    {
        _currentBoatStorage = 0;
    }

    private void BoatStorageUpdate()
    {
        BoatStorage = 10 * (2 * _lM.BoatStorageLevel);
    }
}
