using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingPlatform : MonoBehaviour, IElectric
{
    Action _MoveAction = delegate { };
    [SerializeField] Vector3 _offset;
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
    [SerializeField] GameObject pivot;

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
        float dist = Vector3.Distance(pivot.transform.position, _waypoitns[currentWaypoint].position);
        Vector3 dir = _waypoitns[currentWaypoint].position - (platform.transform.position + _offset);
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
        if (!stop)
        {
            if (dist < arriveMag)
            {
                _myRB2D.velocity = dir * _speed * dist * Time.fixedDeltaTime;
            }
            else
            {
                _myRB2D.velocity = dir * _speed * Time.fixedDeltaTime;
            }
        }
        else if (stop)
        { 
            _myRB2D.velocity = new Vector2(0, 0); 
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
    bool stop = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 27)
        {
            stop = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 27)
            stop = false;
    }

}
