using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatStorageManager : MonoBehaviour
{
    [SerializeField] private LevelManager _lM;

    public int BoatStorage;
    private int _fishInStorage;

    private void Start()
    {
        CalculateBoatStorage();
    }


    private void CalculateBoatStorage()
    {
        BoatStorage = 100 * (_lM.BoatStorageLevel + 2 * _lM.CurrentBoatLevel);
    }


}
