using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Electricty : MonoBehaviour
{
    Action _Action;
    ParticleSystem _particle;
    [SerializeField] List<Transform> _waypoints;
    int current = 0;
    [SerializeField] float _speed;

    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        _particle.Stop();
        _Action = delegate { };
    }
    private void Update()
    {
        _Action();
    }
    void Move()
    {
        var dist = Vector3.Distance(transform.position, _waypoints[current].position);
        if (dist < 0.15f)
        {
            if (current == _waypoints.Count - 1)
            {
                current = 0;
                //_particle.Stop();
                //_Action = delegate { };
                transform.position = _waypoints[0].position;
            }
            else
            {
                current++;
            }
        }
        var dir = _waypoints[current].position - transform.position;
        transform.position += dir.normalized * _speed * Time.deltaTime;
    }
    public void Activate()
    {
        _Action = Move;
        _particle.Play();
    }
}
