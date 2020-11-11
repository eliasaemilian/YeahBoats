using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FishAI : MonoBehaviour
{

    public FishSpawner FishSpawner { private get; set; }

    public float MinFishSpeed { private get; set; }
    public float MaxFishSpeed { private get; set; }

    public int DirectionChangeChance { private get; set; }
    public int DirectionChangeDelay { private get; set; }

    private Vector3 _marker;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _marker = GetRandomMarker();
        _speed = SetRandomSpeed();
        InvokeRepeating("ChanceToChangeDirection", 1, DirectionChangeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        FishMovement();

        if(Vector3.Distance(transform.position, _marker) < 0.1f)
        {
            _marker = GetRandomMarker();
            _speed = SetRandomSpeed();
        }
    }

    private Vector3 GetRandomMarker()
    {
        int rnd = Random.Range(0, FishSpawner.Markers.Count);

        return FishSpawner.Markers[rnd];
    }
    private float SetRandomSpeed()
    {
        return Random.Range(MinFishSpeed, MaxFishSpeed);
    }

    private Vector3 _targetDirection;
    private Vector3 _newDirection;
    private void FishMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _marker, _speed*Time.deltaTime);

        _targetDirection = _marker - transform.position;
        _newDirection = Vector3.RotateTowards(transform.forward, _targetDirection, 10 * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(_newDirection);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fish")
        {
            _marker = GetRandomMarker();
            _speed = SetRandomSpeed();
        }
    }
    private void ChanceToChangeDirection()
    {
        if(Random.Range(0,100) < DirectionChangeChance)
        {
            _marker = GetRandomMarker();
            _speed = SetRandomSpeed();
        }
    }
}
