using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateEvent : UnityEvent<GameObject>
{

}

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private MultiplierManager _MM = null;

    [SerializeField] private float _crateSpawnTime = 5f;
    [SerializeField] private int _crateMaxAmmount = 1;

    public GameObject MapTab;
    public GameObject MultiplierTab;

    private int CratesCount;

    public List<Transform> SpawnPositions;

    public CrateEvent m_CrateEvent;


    private float _timer;
    private int _previousSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        m_CrateEvent = new CrateEvent();
        m_CrateEvent.AddListener(TappedOnCrate);
        m_CrateEvent.AddListener(ClearCrate);
        CratesCount = 0;
        _timer = _crateSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0)
        {
            if(CratesCount < _crateMaxAmmount)
            {
                CrateSpawn();

            }
            _timer = _crateSpawnTime;
        }
    }

    private void CrateSpawn()
    {
        int spawnPosition = Random.Range(0, SpawnPositions.Count);
        while(spawnPosition == _previousSpawnPosition)
        {
            spawnPosition = Random.Range(0, SpawnPositions.Count);
        }

        _previousSpawnPosition = spawnPosition;
        GameObject crate = ObjectPooler.Instance.SpawnFromPool("Crate", SpawnPositions[spawnPosition].position, Quaternion.identity);
        crate.GetComponent<CrateScript>().CS = this;
        CratesCount++;

        Debug.Log("Spawning a crate");
    }

    private void ClearCrate(GameObject crate)
    {
        CratesCount--;

        ObjectPooler.Instance.ReturnToPool("Crate", crate);
        
    }

    public void TappedOnCrate(GameObject crate)
    {
        if(LevelManager.Instance.MapPieces < 4)
        {

        int res = Random.Range(0, 2);
        if(res == 0)
        {
            Debug.Log("Received a Map piece");
            LevelManager.Instance.AddMapPiece();
            MapTab.SetActive(true);

        }
        else
        {
            Debug.Log("Received a multiplier");
            MultiplierTab.SetActive(true);
            _MM.AddTimeToMultiplier(2, 2);
        }
        }
        else
        {
            Debug.Log("Received a multiplier");
            MultiplierTab.SetActive(true);
            _MM.AddTimeToMultiplier(2, 2);
        }
    }
}
