using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancySampling : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float depthBeforeSubmerged = .1f;
    [SerializeField] private float displacementAmount = 3f; // change these for rigidbody properties

    private Vector3[] _positionsSamplePoints;
     
    private float waterDrag; //TODO: move boat parameters to boat class
    [SerializeField]private float waterAngularDrag;

    private Boat myBoat;

    private float _waveHeight;

    void Start()
    {
#if UNITY_EDITOR
        if (transform.childCount == 0)
        {
            Debug.LogWarning("Floating Object " + transform.parent.name + " needs to have at least 1 Sample Position assigned as child of BouyancySampling Script");
            return;
        }
#endif
        // Save local positions for sampling points of this model
        _positionsSamplePoints = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) _positionsSamplePoints[i] = transform.GetChild(i).position;

        // Store Boat Info
        myBoat = GetComponentInParent<BoatHandler>().ThisBoat;
        waterDrag = myBoat.waterDrag;
        waterAngularDrag = myBoat.waterAngularDrag;
        _rb = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < _positionsSamplePoints.Length; i++)
        {
            // apply Gravity
            _rb.AddForceAtPosition(Physics.gravity / _positionsSamplePoints.Length, _positionsSamplePoints[i], ForceMode.Acceleration);
            _waveHeight = WaveManager.Instance.GetWaveHeight(_positionsSamplePoints[i]);

            // if position sampled is underwater apply calculated upwards force
            if (_positionsSamplePoints[i].y < _waveHeight)
            {
                ApplyUpwardsForce(_waveHeight, _positionsSamplePoints[i]);
                Debug.Log("PUSHING POINT " + i);
            }

        }

    }

    /// <summary>
    /// Applies a force in opposition to the water plane and in relation to the objects body parameters
    /// to keep the object afloat
    /// </summary>
    /// <param name="waveHeight">WaveHeight, prrovided by WaveManager at sampled Point</param>
    /// <param name="pos">position of sampled Point</param>
    private void ApplyUpwardsForce(float waveHeight, Vector3 pos)
    {
        float displacementMult = Mathf.Clamp01(waveHeight - pos.y / depthBeforeSubmerged) * displacementAmount;

        _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMult, 0f), pos, ForceMode.Acceleration);
        _rb.AddForce(displacementMult * -_rb.velocity * waterDrag * Time.fixedDeltaTime);
        _rb.AddTorque(displacementMult * -_rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime);

        //    Debug.Log("Pushed up");
    }

    private void OnDrawGizmos()
    {
        if (_positionsSamplePoints.Length <= 0) return;

        for (int i = 0; i < _positionsSamplePoints.Length; i++)
        {
            Gizmos.color = Color.red;
            Vector3 pos = _positionsSamplePoints[i];
            pos.y = WaveManager.Instance.GetWaveHeight(_positionsSamplePoints[i]);
            Gizmos.DrawWireSphere(pos, .25f);
        }
    }
}
