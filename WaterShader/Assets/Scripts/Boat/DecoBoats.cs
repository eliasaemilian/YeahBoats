using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoBoats : MonoBehaviour
{
    [SerializeField] private GameObject _displayBoat1;
    [SerializeField] private GameObject _displayBoat2;
    [SerializeField] private GameObject _displayBoat3;
    [SerializeField] private GameObject _displayBoat4;
    [SerializeField] private GameObject _displayBoat5;

    private GameObject boat = null;
    void Start()
    {
        setupBoat();
    }

    public void UpdateBoat()
    {
        Destroy(boat.gameObject);
        setupBoat();
    }
    private void setupBoat()
    {
        GameObject prefab;
        switch (LevelManager.Instance.CurrentBoatLevel)
        {
            case 1:
                prefab = _displayBoat1;
                break;
            case 2:
                prefab = _displayBoat2;
                break;
            case 3:
                prefab = _displayBoat3;
                break;
            case 4:
                prefab = _displayBoat4;
                break;
            case 5:
                prefab = _displayBoat5;
                break;
            default:
                prefab = _displayBoat1;

                break;
        }
        boat = Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform);
    }
}
