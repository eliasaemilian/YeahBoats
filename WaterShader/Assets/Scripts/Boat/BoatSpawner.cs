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

    public void InstantiateBoat()
    {
        GameObject b = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].BoatPrefab;
        _lM.BoatSkillLevels = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1];
        _lM.MaxAmmountOfFishermen = _lM.BoatLevels.Levels[_lM.CurrentBoatLevel - 1].NPCFishermanAmmount;
        GameObject boat = Instantiate(b, Spawnposition.position, Quaternion.identity);
        boat.transform.parent = Parent.transform;
    }
}
