using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> https://www.youtube.com/watch?v=eL_zHQEju8s


public class FloaterCustomURP : MonoBehaviour
{
    private Rigidbody _rb;
    private float depthBeforeSubmerged = 1f;
    private float displacementAmount = 3f; // change these for rigidbody properties

    public int floatCounter = 4;
    public float waterDrag = .99f;
    public float waterAngularDrag = .5f;

    private Material _waterMat;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _waterMat = GetComponentInParent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.AddForceAtPosition(Physics.gravity / floatCounter, transform.position, ForceMode.Acceleration);
        Debug.Log("Gravity");
        float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x);
     //   waveHeight = _waterMat.GetFloat("");


        if (transform.position.y < waveHeight)
        {
            PushUp(waveHeight);
        }
    }


    private void PushUp(float waveHeight)
    {
            float displacementMult = Mathf.Clamp01(waveHeight - transform.position.y / depthBeforeSubmerged) * displacementAmount;

            _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMult, 0f), transform.position, ForceMode.Acceleration);
            _rb.AddForce(displacementMult * -_rb.velocity * waterDrag * Time.fixedDeltaTime);
            _rb.AddTorque(displacementMult * -_rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime);

        Debug.Log("Pushed up");
    }
}
