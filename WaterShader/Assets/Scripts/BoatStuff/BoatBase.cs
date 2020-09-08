using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets up the boat and its components
/// </summary>
public class BoatBase : MonoBehaviour
{
    [SerializeField] private BoatScriptable _boatScriptable;
    [SerializeField] private NPCSpotsScript _nPCSpots;
    [SerializeField] private GameObject _nPCFishermanPrefab;

    public List<GameObject> SpawnPoints = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //AddFisherman();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddFisherman();
        }
    }
    private bool AddFisherman()
    {
        if (SpawnPoints.Count < _boatScriptable.NPCSpawnPointAmmount)
        {
            GameObject gO = _nPCSpots._spots[SpawnPoints.Count];
            SpawnPoints.Add(gO);

            GameObject fisherman = Instantiate(_nPCFishermanPrefab,gO.transform.position,gO.transform.rotation, gO.transform);
            return true;
        }
        else return false;
    }

}
