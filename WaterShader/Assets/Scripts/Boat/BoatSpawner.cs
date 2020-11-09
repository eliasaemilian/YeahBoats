using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpawner : MonoBehaviour
{
    private LevelManager _lM;
    public Transform Spawnposition;
    public GameObject Parent;


    void Start()
    {
        _lM = LevelManager.Instance;
        InstantiateBoat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateBoat()
    {
        Debug.Log("Current Boat Level : " + _lM.CurrentBoatLevel);
        GameObject b = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].BoatPrefab;
        _lM.BoatSkillLevels = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1];
        _lM.MaxAmmountOfFishermen = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].NPCFishermanAmmount;
        GameObject boat = Instantiate(b, Spawnposition.position, Quaternion.identity,Parent.transform);
        boat.transform.parent = this.transform;
        //_nPCSpots = boat.gameObject.GetComponentInChildren<NPCSpotsScript>();
    }
}
