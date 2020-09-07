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

    public List<Transform> SpawnPoints = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        //Adds the spawnpoints to te list
        for (int i = 0; i < _boatScriptable.NPCSpawnPointAmmount; i++)
        {
            SpawnPoints.Add(_nPCSpots._spots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
