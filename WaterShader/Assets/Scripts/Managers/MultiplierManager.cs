using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierManager : MonoBehaviour
{

    private int _baseMultiplier = 1;
    private int _multiplier;
    private int Multiplier 
    {
        get
        {
            return _multiplier;
        }
        set 
        {
            if (value == _multiplier) return;
            _multiplier = value;
            LevelManager.Instance.Multiplier = value; 
        } 
    }
    public float CatchSpeedMultiplier;

    private float _multiplierTimer;
    private float _catchSpeedMultiplierTimer;

    // Start is called before the first frame update
    void Start()
    {
        Multiplier = LevelManager.Instance.Multiplier;
        CatchSpeedMultiplier = LevelManager.Instance.CatchSpeedMultiplier;
        _multiplierTimer = 0;
        _catchSpeedMultiplierTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_multiplierTimer >= 0)
        {
            _multiplierTimer -= Time.deltaTime;
        }
        if(_catchSpeedMultiplierTimer >= 0)
        {
            //for later
        }
        if (_multiplierTimer <= 0) Multiplier = _baseMultiplier;
    }

    public void AddTimeToMultiplier(float time, int multiplyAmmount)
    {
        if (_multiplierTimer < 0) _multiplierTimer = 0;
        _multiplierTimer += time*60;
        Multiplier = multiplyAmmount;
    }

}
