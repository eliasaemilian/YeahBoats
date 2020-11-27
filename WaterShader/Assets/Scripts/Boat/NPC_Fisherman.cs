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

    private Animator _animator;
    private UI_JoystickHandler _joystickHandler;

    

    public BoatBase BB;

    //MVP
    private Renderer rend;

    public float CatchSpeed { get; set; }
    public float Multiplier;


    [SerializeField] private float _catchSpeedBase = 10;

    [SerializeField]private float _timer;
    private float _timerTapMultiplier;

    private Camera _uiCamera;
    private UI_InputHandler _inputHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        Multiplier = _lM.CatchSpeedMultiplier;
        CatchSpeed = _catchSpeedBase * (_lM.FishingRodLevel) * Multiplier;

        _timer = 60/CatchSpeed;
        _timerTapMultiplier = 1.5f;
        _lM.NPCUpdate.AddListener(UpdateValues);
        BB.FishingSpeedup.AddListener(ReduceTimer);
        _lM.MultiplierUpdate.AddListener(UpdateValues);
        
        //MVP
        rend = GetComponentInChildren<Renderer>();

        _animator = GetComponentInChildren<Animator>();
        _joystickHandler = FindObjectOfType<UI_JoystickHandler>();
        UI_JoystickHandler.JoystickStateChanged.AddListener(UpdateFishingState);


        _inputHandler = FindObjectOfType<UI_InputHandler>();
        if (_uiCamera == null) _uiCamera = _inputHandler.UICamera;

    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetBool("FishingMode"))
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                CatchAFish();
                _timer =  60/CatchSpeed;
            }
        }
       
    }

    private void CatchAFish()
    {
        float FishCost = _fM.GetFish();
        _mM.AddMoney(FishCost);
        //StartCoroutine(NPCAnim());

        Vector3 _offset = transform.position + new Vector3(0, 2, 0);
        //Popup
        Vector3 transPos = Camera.main.WorldToScreenPoint(_offset);
        transPos = _uiCamera.ScreenToWorldPoint(transPos);
        //transPos.y += 2;
        _pM.CallCoinPopup(transPos, (int)FishCost);

        _animator.SetTrigger("CaughtFish");
    }

    public void UpdateValues()
    {
        Multiplier = _lM.Multiplier;
        CatchSpeed = _catchSpeedBase * (_lM.FishingRodLevel) * Multiplier;
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
     //   Debug.Log("Reducing timer");
        _timer /= _timerTapMultiplier;
    }

    private void UpdateFishingState()
    {
        if (_joystickHandler.JoystickStateClosed)
        {
            _animator.SetBool("FishingMode", true);
        }
        else
        {
            _animator.SetBool("FishingMode", false);
        }
    }
}
