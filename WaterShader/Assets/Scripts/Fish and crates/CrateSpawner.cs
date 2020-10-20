using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateEvent : UnityEvent<GameObject>
{

}

public class CrateSpawner : MonoBehaviour
{

    [SerializeField] private GameObject _cratePrefab;

    [SerializeField] private float _crateSpawnTime;
    [SerializeField] private int _crateMaxAmmount;

    private List<GameObject> CratesList;

    public List<Transform> SpawnPositions;

    public CrateEvent m_CrateEvent;


    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        m_CrateEvent = new CrateEvent();
        m_CrateEvent.AddListener(TappedOnCrate);
        m_CrateEvent.AddListener(ClearCrate);
        CratesList = new List<GameObject>();
        _timer = _crateSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0)
        {
            if(CratesList.Count < _crateMaxAmmount)
            {
                CrateSpawn();

            }
            _timer = _crateSpawnTime;
        }
    }

    private void CrateSpawn()
    {
        GameObject go = Instantiate(_cratePrefab, SpawnPositions[Random.Range(0, SpawnPositions.Count)].position, Quaternion.identity);
        go.GetComponent<CrateScript>().CS = this;
        CratesList.Add(go);
    }

    private void ClearCrate(GameObject crate)
    {
        CratesList.Remove(crate);

        Destroy(crate.gameObject);
        
    }

    public void TappedOnCrate(GameObject crate)
    {
        int res = Random.Range(0, 2);
        if(res == 0)
        {
            // I received a map piece
            Debug.Log("Received a Map piece");

        }
        else
        {
            // I received a multiplyer
            Debug.Log("Received a multiplier");
        }
    }
}
