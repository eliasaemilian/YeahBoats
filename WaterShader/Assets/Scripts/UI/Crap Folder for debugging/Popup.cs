using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private float _upSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0,_upSpeed,0) * Time.deltaTime;
        transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f) * Time.deltaTime;
    }

    public void Setup(int value)
    {
        _text.text = "+ " + value.ToString();
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(this.gameObject);
    }
    IEnumerator MovementCoroutine(float time, float value)
    {
        yield return new WaitForSeconds(time);

        _upSpeed = value;
    }
}
