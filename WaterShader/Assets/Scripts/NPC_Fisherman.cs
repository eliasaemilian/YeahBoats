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

    public float CatchSpeed;
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

        Debug.Log("CatchspeedBase: " + _catchSpeedBase);
        Debug.Log("FishermanLevel: " + _lM.NPCFishermanLevel);
        Debug.Log("Multiplier: " + Multiplier);
        Debug.Log("Multiplier: " + CatchSpeed);
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

}
