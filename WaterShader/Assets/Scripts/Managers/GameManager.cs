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

    public BoatBase Boat;
    [SerializeField] private GameObject _boatPrefab;

    public UnityEvent FishingSpeedup;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject boat = Instantiate(_boatPrefab,transform.position, Quaternion.identity);
        Boat = boat.GetComponent<BoatBase>();
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
}
