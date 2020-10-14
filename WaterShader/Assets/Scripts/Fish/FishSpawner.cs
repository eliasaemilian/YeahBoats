using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawner : MonoBehaviour
{

    public List<FishAI> FishList { get; private set; }
    public List<Vector3> Markers { get; private set; }
    

    [SerializeField] private List<GameObject> _fishPrefabs;

    [Header("Fish Setup")]

    [SerializeField] private float _spawnAmmount;
    [SerializeField] private float _fishDecayTime;
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

    // Start is called before the first frame update
    void Start()
    {
        FishList = new List<FishAI>();
        Markers = new List<Vector3>();
        SpawnMarkers();
        SpawnFishes();
        InvokeRepeating("RemoveFish",_initialDelay,_removeTimer);

    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void RespawnFishes()
    {

    }

    private void SpawnMarkers()
    {
        for (int i = 0; i < _markerAmmount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-(_x / 2), (_x / 2)),transform.position.y + Random.Range(-(_y / 2), (_y / 2)),transform.position.z+ Random.Range(-(_z / 2), (_z / 2)));

            Markers.Add(pos);
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
