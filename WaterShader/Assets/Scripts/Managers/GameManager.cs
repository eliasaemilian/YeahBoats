using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Gamemanager sets up the level for the boat etc
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private LevelManager _lM;

    public BoatBase Boat;
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private LevelStorageScriptable _boatLevels;
    public Data DataContainer;

    public UnityEvent FishingSpeedup;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
        DataContainer = GetComponent<Data>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _lM = LevelManager.Instance;

        InstantiateBoat();
    }

    // Update is called once per frame
    void Update()
    {
        //MVP for testing
        if (Input.GetKeyDown(KeyCode.S))
        {
            TapToSpeeUpCatch();
        }
    }

    // Call this when the player taps on the screen to speed up the fishing process
    public void TapToSpeeUpCatch()
    {
        FishingSpeedup.Invoke();
    }

    //Setting up boat
    private void InstantiateBoat()
    {
        GameObject b = _boatLevels.Levels[_lM.BoatLevel - 1].BoatPrefab;
        GameObject boat = Instantiate(b, transform.position, Quaternion.identity);
        Boat = boat.GetComponent<BoatBase>();
        Debug.Log("Loading boat of level : " + _boatLevels.Levels[_lM.BoatLevel - 1].Level);
    }

    
}
