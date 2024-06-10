using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SawPathingNetwork : NetworkBehaviour
{
    [SerializeField] List<Transform> _waypoints = new List<Transform>();
    [SerializeField] private int _currentWaypoint = 0;
    [SerializeField] private int _maxWaypoints;
    [SerializeField] private GameObject _saw;
    [SerializeField] private float _speed;

    void Start()
    {
        _maxWaypoints = _waypoints.Count - 1;
    }
    void Update()
    {
        Movement();
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
        Vector3 dir = _waypoints[_currentWaypoint].position -_saw.transform.position;
        dir.Normalize();
        _saw.transform.position += dir * _speed * Time.deltaTime;
    }
}
