using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingPlatform : MonoBehaviour, IElectric
{
    Action _MoveAction = delegate { };

    public GameObject platform;
    [Header("Waypoints")]
    [SerializeField] private List<Transform> _waypoitns;
    public int currentWaypoint = 0;
    [Header("Data")]
    [SerializeField] private float _speed = 0, _baseSpeed = 3;
    public float arriveMag = 0.5f;
    public int maxLenght = 0;
    private int _senseDir = 1;
    private Rigidbody2D _myRB2D;
    [SerializeField] private bool _forceTurnOn = false;

    private void Start()
    {
        maxLenght = _waypoitns.Count - 1;
        _speed = _baseSpeed;
        _myRB2D = platform.GetComponent<Rigidbody2D>();
        if (_forceTurnOn)
            TurnOn();
    }

    private void FixedUpdate()
    {
        _MoveAction();
    }
    void Movement()
    {
        float dist = Vector3.Distance(platform.transform.position, _waypoitns[currentWaypoint].position);
        Vector3 dir = _waypoitns[currentWaypoint].position - platform.transform.position;
        dir.Normalize();
        if (dist < 0.1)
        {
            if (currentWaypoint == maxLenght)
            {
                if (_senseDir == 1)
                {
                    _senseDir = -1;
                    maxLenght = 0;
                }
                else
                {
                    _senseDir = 1;
                    maxLenght = _waypoitns.Count - 1;
                }
            }
            currentWaypoint += _senseDir;
        }
        if (dist < arriveMag)
        {
            _myRB2D.velocity = dir * _speed * dist * Time.fixedDeltaTime;
            //platform.transform.position += dir * _speed * dist * Time.deltaTime;
        }
        else
        {
            _myRB2D.velocity = dir * _speed * Time.fixedDeltaTime;
            //platform.transform.position += dir * _speed * Time.deltaTime;
        }
    }

    public void TurnOn()
    {
        _MoveAction = Movement;
    }

    public void TurnOff()
    {
        _MoveAction = delegate { };
        _myRB2D.velocity = new Vector2(0, 0);
    }


}
