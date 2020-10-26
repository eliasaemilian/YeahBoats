using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private LevelManager _lM = null;
    [SerializeField] private MoneyManager _mM = null;

    [SerializeField] private GameObject _coinPopup = null;
    [SerializeField] private GameObject _FishPopup = null;

    [SerializeField] private Transform _PopupContainer = null;

    [SerializeField] private Camera _camera = null;

    private Transform _canvas;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _canvas = FindObjectOfType<Canvas>().transform;
        if (_canvas == null) Debug.LogError("You fucked up where is Canvas??");
    }


    public void CallCoinPopup(Vector3 position, int value)
    {
        GameObject g = Instantiate(_coinPopup, position, _camera.transform.rotation);
        g.transform.SetParent(_PopupContainer);
        g.GetComponent<Popup>().Setup(value);
    }
    public void CallFishPopup(Vector3 position, int value)
    {
        GameObject g = Instantiate(_FishPopup, position, _camera.transform.rotation);
        g.transform.SetParent(_PopupContainer);
        g.GetComponent<Popup>().Setup(value);
    }

    public void CallFishAndCoinPopup(Vector3 position)
    {
        int cost = CalculateCost();
        StartCoroutine(CallCoroutine(position,cost, _lM.TapFishLevel));

        _mM.AddMoney(cost);
    }

    IEnumerator CallCoroutine(Vector3 position, int value, int fish)
    {
        CallFishPopup(position, fish);
        yield return new WaitForSeconds(0.4f);
        CallCoinPopup(position, value);
    }

    private int TapCost()
    {
        return (int)(_lM.TapCoinLevel * 1.4f);
    }
    private int CalculateCost()
    {
        int cost = TapCost() * _lM.TapFishLevel * _lM.Multiplier;
        return cost;
    }
}
