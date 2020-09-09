using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Fisherman, catches fish periodically
/// </summary>
public class NPC_Fisherman : MonoBehaviour
{
    private LevelManager _lM = LevelManager.Instance;
    private MoneyManager _mM = MoneyManager.Instance;
    private FishManager _fM = FishManager.Instance;

    private float CatchSpeed { get; set; }
    public int Multiplier;

    [SerializeField] private float _catchSpeedBase = 10;

    private float _timer;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        Multiplier = _lM.Multiplyer;
        CatchSpeed = _catchSpeedBase * (_lM.NPCFishermanLevel) * Multiplier;
        _timer = 0;
        _lM.NPCUpdate.AddListener(UpdateValues);

    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= (60 / CatchSpeed))
        {
            Debug.Log("Got a fish!");
            CatchAFish();
            _timer = 0;
        }
    }

    private void CatchAFish()
    {
        _mM.AddMoney( _fM.GetFish());
    }

    public void UpdateValues()
    {
        Multiplier = _lM.Multiplyer;
        CatchSpeed = _catchSpeedBase * (_lM.NPCFishermanLevel) * Multiplier;
    }
}
