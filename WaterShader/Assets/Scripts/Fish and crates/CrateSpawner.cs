using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateEvent : UnityEvent<GameObject> { }

public class CrateSpawner : MonoBehaviour
{
    public CrateEvent CrateSpawnEvent;

    [SerializeField] private MultiplierManager _MM = null;

    [SerializeField] private float _crateSpawnTime = 5f;
    [SerializeField] private int _crateMaxAmmount = 1;

    [SerializeField] private GameObject _mapFoundPopup = null;
    [SerializeField] private GameObject _multiplierFoundPopup = null;
    [SerializeField] private AudioClip _crateFoundAudioclip = null;

    private Transform[] _spawnPositions;

    private float _timer;
    private int _cratesCount;
    private int _previousSpawnPosition;

    private bool _diableForNoSpawnPointsInScene;

    void Start()
    {
        CrateSpawnEvent = new CrateEvent();
        CrateSpawnEvent.AddListener(TappedOnCrate);
        CrateSpawnEvent.AddListener(ClearCrate);
        _cratesCount = 0;
        _timer = _crateSpawnTime;

        _spawnPositions = GetComponentsInChildren<Transform>();
        if (_spawnPositions.Length == 0)
        {
            Debug.LogError("Crate Spawner has no SpawnPoints assigned as child Gameobjects");
            _diableForNoSpawnPointsInScene = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_diableForNoSpawnPointsInScene) return;

        _timer -= Time.deltaTime;
        if(_timer <= 0)
        {
            if(_cratesCount < _crateMaxAmmount)
            {
                CrateSpawn();
            }
            _timer = _crateSpawnTime;
        }
    }

    private void CrateSpawn()
    {
        int spawnPosition = Random.Range(0, _spawnPositions.Length);
        while(spawnPosition == _previousSpawnPosition)
        {
            spawnPosition = Random.Range(0, _spawnPositions.Length);
        }

        _previousSpawnPosition = spawnPosition;
        GameObject crate = ObjectPooler.Instance.SpawnFromPool("Crate", _spawnPositions[spawnPosition].position, Quaternion.identity);
        crate.GetComponent<CrateScript>().CS = this;
        crate.GetComponent<CrateScript>().FoundCrateSound = _crateFoundAudioclip;
        _cratesCount++;

    }

    private void ClearCrate(GameObject crate)
    {
        _cratesCount--;

        ObjectPooler.Instance.ReturnToPool("Crate", crate);
        
    }

    public void TappedOnCrate(GameObject crate)
    {
        if(LevelManager.Instance.MapPieces < 4)
        {

        int res = Random.Range(0, 2);
        if(res == 0)
        {
            LevelManager.Instance.AddMapPiece();
            _mapFoundPopup.SetActive(true);

        }
        else
        {
            _multiplierFoundPopup.SetActive(true);
            _MM.AddTimeToMultiplier(2, 2);
        }
        }
        else
        {
            _multiplierFoundPopup.SetActive(true);
            _MM.AddTimeToMultiplier(2, 2);
        }
    }

}
