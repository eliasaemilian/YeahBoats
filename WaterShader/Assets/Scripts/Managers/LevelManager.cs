using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// LevelManager keeps track of the level of the Boat, Fisherman, Rod etc...
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int MapLevel;
    public int BoatLevel;
    public int NPCFishermanLevel;
    public int FishingRodLevel;
    public int Multiplyer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }

        TMPLevelSetup();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TMPLevelSetup()
    {
        MapLevel = 1;
        BoatLevel = 1;
        NPCFishermanLevel = 1;
        FishingRodLevel = 1;
        Multiplyer = 1;
    }
}
