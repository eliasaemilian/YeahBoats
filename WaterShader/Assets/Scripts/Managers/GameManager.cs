using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
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
    //[SerializeField] private GameObject _boatPrefab = null;
    //[SerializeField] private LevelStorageScriptable _boatLevels = null;
    public Savedata DataContainer;

    public UnityEvent FishingSpeedup;
    void Awake()
    {
        Instance = this;
        
        DataContainer = GetComponent<Savedata>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _lM = LevelManager.Instance;

       // InstantiateBoat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 130, 150, 20), "Tap on Screen"))
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
    //private void InstantiateBoat()
    //{
    //    GameObject b = _boatLevels.Levels[_lM.BoatLevel - 1].BoatPrefab;
    //    GameObject boat = Instantiate(b, transform.position, Quaternion.identity);
    //    Boat = boat.GetComponent<BoatBase>();
    //    Debug.Log("Loading boat of level : " + _boatLevels.Levels[_lM.BoatLevel - 1].Level);
    //}

    
}
