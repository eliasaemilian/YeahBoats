using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Gamemanager sets up the level for the boat etc
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BoatBase Boat;
    [SerializeField] private GameObject _boatPrefab;
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
        
    }
}
