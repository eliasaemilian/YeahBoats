using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Managesh the fishes, their cost and rarity
/// </summary>
public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    private float _fishValue;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _fishValue = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //does all needed to get fish
    public float GetFish()
    {
        //TODO: animation for fish etc
        //TODO: rare fish catch

        return _fishValue;
    }
}
