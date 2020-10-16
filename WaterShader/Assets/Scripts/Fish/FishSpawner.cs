using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class FishSpawner : MonoBehaviour
{

    public List<FishAI> FishList { get; private set; }
    public List<Vector3> Markers { get; private set; }
    

    [SerializeField] private List<GameObject> _fishPrefabs;

    [Header("Fish Setup")]

    [SerializeField] private float _spawnAmmount;
    [SerializeField] private float _minFishSpeed, _maxFishSpeed;

    [SerializeField] private int _directionChangeChance;
    [SerializeField] private int _directionChangeDelay;

    [Header("Spawner Setup")]
    [Header("Spawner Size")]

    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private float _z;
    [Header("Markers")]

    [SerializeField] private int _markerAmmount;

    [Header("Fish Removal")]
    [SerializeField] private float _initialDelay;
    [SerializeField] private float _removeTimer;

    private bool _isBoatHere;

    private float _multiplier;
    public float Multiplier 
    {
        get
        {
            return _multiplier;
        }
        
        set 
        {
            if (_multiplier == value) return;
            _multiplier = value;
            LevelManager.Instance.CatchSpeedMultiplier = Multiplier;
            LevelManager.Instance.NPCUpdate.Invoke();
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        FishList = new List<FishAI>();
        Markers = new List<Vector3>();
        SpawnMarkers();
        SpawnFishes();
        //RemoveFishLoop(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Respawning process begins");
        }

        if(FishList.Count == 0)
        {
            RemoveFishLoop(false);
        }

        if((FishList.Count / _spawnAmmount)* 100 <= 20 && _isBoatHere)
        {
            Multiplier = 0.5f;
        }
        else
        {
            Multiplier = 1;
        }
    }

    //Spawns fishes
    private void SpawnFishes()
    {
        for (int i = 0; i < _spawnAmmount; i++)
        {
            Vector3 pos = Markers[Random.Range(0,Markers.Count)];
            GameObject prefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Count)];
            FishAI fish = Instantiate(prefab, pos, Quaternion.identity).GetComponent<FishAI>();
            fish.FishSpawner = this;
            fish.MinFishSpeed = _minFishSpeed;
            fish.MaxFishSpeed = _maxFishSpeed;
            fish.DirectionChangeChance = _directionChangeChance;
            fish.DirectionChangeDelay = _directionChangeDelay;

            FishList.Add(fish);
        }
    }
    private void SpawnOneFish()
    {
        Vector3 pos = Markers[Random.Range(0, Markers.Count)];
        GameObject prefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Count)];
        FishAI fish = Instantiate(prefab, pos, Quaternion.identity).GetComponent<FishAI>();
        fish.FishSpawner = this;
        fish.MinFishSpeed = _minFishSpeed;
        fish.MaxFishSpeed = _maxFishSpeed;
        fish.DirectionChangeChance = _directionChangeChance;
        fish.DirectionChangeDelay = _directionChangeDelay;

        FishList.Add(fish);
        Debug.Log("Spawned one fish");
    }

    private void RespawnFishes(bool state)
    {
        if (state)
        {
            if(FishList.Count < _spawnAmmount)
            {
                InvokeRepeating("SpawnOneFish", 5, 5);
            }
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            CancelInvoke("SpawnOneFish");
        }
    }

    IEnumerator RespawnCoroutine()
    {
       while(FishList.Count < _spawnAmmount)
        {
            yield return new WaitForSeconds(1);
        }
        CancelInvoke("SpawnOneFish");

    }

    private void SpawnMarkers()
    {
        for (int i = 0; i < _markerAmmount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-(_x / 2), (_x / 2)),transform.position.y + Random.Range(-(_y / 2), (_y / 2)),transform.position.z+ Random.Range(-(_z / 2), (_z / 2)));

            Markers.Add(pos);
        }
    }

    private void RemoveFishLoop(bool state)
    {
        if (state)
        {
            InvokeRepeating("RemoveFish", _initialDelay, _removeTimer);
            Debug.Log("Fish Removal Started");
        }
        else
        {
            CancelInvoke("RemoveFish");
            Debug.Log("Fish Removal Stopped");

        }

    }
    private void RemoveFish()
    {
        if(FishList.Count > 0)
        {

        Destroy(FishList[FishList.Count - 1].gameObject);
        FishList.RemoveAt(FishList.Count - 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Boat")
        {
            Debug.Log("Boat entered frishing zone");
            _isBoatHere = true;
            RemoveFishLoop(true);
            RespawnFishes(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Boat")
        {
            Debug.Log("Boat exit frishing zone");
            _isBoatHere = false;
            RemoveFishLoop(false);
            RespawnFishes(true);



        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_x, _y, _z));

        if(Markers != null)
        {

        for (int i = 0; i < Markers.Count; i++)
        {
            Gizmos.DrawSphere(Markers[i], 0.5f);
        }
        }
    }
}
