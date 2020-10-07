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
    private DataManager _dM = DataManager.Instance;
    private PopupManager _pM = PopupManager.Instance;

    private Vector3 _offset;

    public BoatBase BB;

    //MVP
    private Renderer rend;

    public float CatchSpeed { get; set; }
    public int Multiplier;


    [SerializeField] private float _catchSpeedBase = 10;

    [SerializeField]private float _timer;
    private float _timerTapMultiplier;

    private Camera _uiCamera;
    private UI_InputHandler _inputHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        Multiplier = _lM.Multiplier;
        CatchSpeed = _catchSpeedBase * (_lM.NPCFishermanLevel) * Multiplier;

        _timer = 60/CatchSpeed;
        _timerTapMultiplier = 1.5f;
        _lM.NPCUpdate.AddListener(UpdateValues);
        BB.FishingSpeedup.AddListener(ReduceTimer);
        _offset = transform.position + new Vector3(0, 2, 0);
        
        rend = GetComponentInChildren<Renderer>();

        _inputHandler = FindObjectOfType<UI_InputHandler>();
        if (_uiCamera == null) _uiCamera = _inputHandler.UICamera;

    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            Debug.Log("Got a fish!");
            CatchAFish();
            _timer =  60/CatchSpeed;
        }
    }

    private void CatchAFish()
    {
        float FishCost = _fM.GetFish();
        _mM.AddMoney(FishCost);
        StartCoroutine(NPCAnim());

        //Popup
        Debug.Log("Calling Popup");
        Vector3 transPos = Camera.main.WorldToScreenPoint(_offset);
        transPos = _uiCamera.ScreenToWorldPoint(transPos);
        _pM.CallCoinPopup(transPos, (int)FishCost);

        
    }

    public void UpdateValues()
    {
        Multiplier = _lM.Multiplier;
        CatchSpeed = _catchSpeedBase * (_lM.NPCFishermanLevel) * Multiplier;
    }

    private IEnumerator NPCAnim()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        rend.material.color = Color.green;


    }
     // gets called to speed up fishing
    public void ReduceTimer()
    {
        _timer /= _timerTapMultiplier;
    }
}
