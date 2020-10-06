using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private GameObject _coinPopup;
    [SerializeField] private GameObject _FishPopup;
    [SerializeField] private Camera _camera;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = new Vector3(0, 3, 0);
            CallFishAndCoinPopup(pos, 15,1);
        }
    }

    public void CallCoinPopup(Vector3 position, int value)
    {
        GameObject g = Instantiate(_coinPopup, position, _camera.transform.rotation);
        g.GetComponent<Popup>().Setup(value);
    }
    public void CallFishPopup(Vector3 position, int value)
    {
        GameObject g = Instantiate(_FishPopup, position, _camera.transform.rotation);
        g.GetComponent<Popup>().Setup(value);
    }

    public void CallFishAndCoinPopup(Vector3 position, int value, int fish)
    {
        StartCoroutine(CallCoroutine(position, value,fish));
    }

    IEnumerator CallCoroutine(Vector3 position, int value, int fish)
    {
        CallFishPopup(position, fish);
        yield return new WaitForSeconds(0.4f);
        CallCoinPopup(position, value);
    }
}
