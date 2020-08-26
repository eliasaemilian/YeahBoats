using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> https://www.youtube.com/watch?v=eL_zHQEju8s


public class Floater : MonoBehaviour
{
    private Rigidbody _rb;
    private float depthBeforeSubmerged = 1f;
    private float displacementAmount = 3f; // change these for rigidbody properties

    private int floatCounter;
    private float waterDrag ;
    private float waterAngularDrag;

    private Boat myBoat;

    private float _waveHeight;

    // Start is called before the first frame update
    void Start()
    {
        myBoat = GetComponentInParent<BoatHandler>().ThisBoat;
       
        floatCounter = myBoat.floatCounter;
        waterDrag = myBoat.waterDrag;
        waterAngularDrag = myBoat.waterAngularDrag;
        _rb = GetComponentInParent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.AddForceAtPosition(Physics.gravity / floatCounter, transform.position, ForceMode.Acceleration);
        // Debug.Log("Gravity");
        _waveHeight = WaveManager.Instance.GetWaveHeight(transform.position);
        //   waveHeight = _waterMat.GetFloat("");

        if (transform.position.y < _waveHeight )
        {
            PushUp(_waveHeight);
            Debug.Log("PUSH");
        }
    }


    private void PushUp(float waveHeight)
    {
            float displacementMult = Mathf.Clamp01(waveHeight - transform.position.y / depthBeforeSubmerged) * displacementAmount;

            _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMult, 0f), transform.position, ForceMode.Acceleration);
            _rb.AddForce(displacementMult * -_rb.velocity * waterDrag * Time.fixedDeltaTime);
            _rb.AddTorque(displacementMult * -_rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime);

    //    Debug.Log("Pushed up");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        pos.y = _waveHeight;
        Gizmos.DrawWireSphere(pos, .25f);
    }
}
