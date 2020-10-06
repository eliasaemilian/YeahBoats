using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopup : MonoBehaviour
{
    [SerializeField] private Transform CoinPopupPrefab;
    [SerializeField] GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        Transform g = Instantiate(CoinPopupPrefab, transform.position, Camera.transform.rotation);
        g.GetComponent<Popup>().Setup(25);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform g = Instantiate(CoinPopupPrefab, transform.position, Camera.transform.rotation);
            g.GetComponent<Popup>().Setup(25);
        }
    }

    
}
