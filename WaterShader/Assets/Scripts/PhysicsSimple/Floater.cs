using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> https://www.youtube.com/watch?v=eL_zHQEju8s


public class Floater : MonoBehaviour
{
    private Rigidbody _rb;
    private float depthBeforeSubmerged = 1f;
    private float displacementAmount = 3f; // change these for rigidbody properties

    public int floatCounter = 4;
    public float waterDrag = .99f;
    public float waterAngularDrag = .5f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.AddForceAtPosition(Physics.gravity, transform.position / floatCounter, ForceMode.Acceleration);

        float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x);
      
        
        if (transform.position.y < waveHeight)
        {
            float displacementMult = Mathf.Clamp01(waveHeight -transform.position.y / depthBeforeSubmerged) * displacementAmount;

            _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMult, 0f), transform.position, ForceMode.Acceleration);
            _rb.AddForce(displacementMult * -_rb.velocity * waterDrag * Time.fixedDeltaTime);
            _rb.AddTorque(displacementMult * -_rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime);
        }
    }
}
