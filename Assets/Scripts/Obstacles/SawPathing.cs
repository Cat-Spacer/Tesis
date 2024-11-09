using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawPathing : MonoBehaviour, IActivate
{
    Action ActivatedAction = delegate {  };
    [SerializeField] List<Transform> _waypoints = new List<Transform>();
    [SerializeField] private int _currentWaypoint = 0;
    [SerializeField] private int _maxWaypoints;
    [SerializeField] private Saw _saw;
    [SerializeField] private float _speed;
    
    [SerializeField] private bool isOn;
    void Start()
    {
        _maxWaypoints = _waypoints.Count - 1;
        if (isOn)
        {
            Activate();
        }
    }
    void Update()
    {
        ActivatedAction();
    }
    public void Activate()
    {
        isOn = true;
        _saw.StartSpinning();
        ActivatedAction = Movement;
    }

    public void Desactivate()
    {
        isOn = false;
        _saw.StopSpinning();
        ActivatedAction = delegate {  };
    }
    void Movement()
    {
        float dist = Vector3.Distance(_saw.transform.position, _waypoints[_currentWaypoint].position);
        if (dist < 0.01f)
        {
            if (_currentWaypoint == _maxWaypoints)
            {
                _currentWaypoint = 0;
            }
            else _currentWaypoint++;
        }

        var position = _saw.transform.position;
        Vector3 dir = _waypoints[_currentWaypoint].position -position;
        dir.Normalize();
        position += dir * (_speed * Time.deltaTime);
        _saw.transform.position = position;
    }

}
