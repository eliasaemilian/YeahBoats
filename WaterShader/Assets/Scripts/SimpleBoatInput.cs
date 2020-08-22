using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class SimpleBoatInput : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] float _speed = 3f;
    [SerializeField] float _rSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) //Accelerate
        {
            _rb.AddForce(Vector3.forward * _speed * Time.deltaTime, ForceMode.Acceleration);
            Debug.Log("Forward");
        }
        else if (Input.GetKey(KeyCode.S)) // Slowdown
        {
            _rb.AddForce(Vector3.back * _speed * Time.deltaTime, ForceMode.Acceleration);

        }

        transform.Rotate(0, Input.GetAxis("Horizontal") * _rSpeed * Time.deltaTime, 0);

        //if (Input.GetKey(KeyCode.A)) // Turn Left
        //{
        //    transform.Rotate(Vector3.up, 90f);
        //}
        //else if (Input.GetKey(KeyCode.D)) // Turn Right
        //{
        //    transform.Rotate(Vector3.up, -90f);
        //}


    }
}
