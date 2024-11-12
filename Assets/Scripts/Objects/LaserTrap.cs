using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserTrap : MonoBehaviour, IActivate
{
    [SerializeField] LineRenderer _line;
    [SerializeField] private Transform linePoint;
    [SerializeField] LayerMask _hitLayerMask;
    [SerializeField] ParticleSystem[] _particles;
    [SerializeField] ParticleSystem[] _particles2;

    private bool _on;
    bool _firstStart;
    [SerializeField] bool start;
    BoxCollider2D coll;
    private LineRenderer _myLineConnection;
    [SerializeField] private float distance;
    void Start()
    {
        _firstStart = true;
        coll = GetComponent<BoxCollider2D>();
        CreateLaser();
        if(start) TurnOn();
        else TurnOff();

    }
    void CreateLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(_particles2[0].transform.position, -transform.up, distance, _hitLayerMask);
        if (hit)
        {
            foreach (var particle2 in _particles2)
            {
                particle2.Play();
            }
            _line.SetPosition(0, linePoint.position);
            _line.SetPosition(1, hit.point);
            float dist = Vector2.Distance(transform.position, hit.point);
            var center = dist *.5f;
            coll.offset = new Vector2(0, -center);
            coll.size = new Vector2(0.1f, dist);
            
            foreach (var particle in _particles)
            {
                particle.Play();
                particle.transform.position = hit.point;
            }
        }
    }
    void TurnOn()
    {
        _on = true;
        _line.enabled = true;
        _firstStart = false;
        coll.enabled = true;
        foreach (var particle2 in _particles2) particle2.Play();
        foreach (var particle in _particles) particle.Play();
    }
    void TurnOff()
    {
        _firstStart = false;
        _on = false;
        _line.enabled = false;
        coll.enabled = false;
        foreach (var particle in _particles)
        {
            particle.Stop();
        }
        foreach (var particle2 in _particles2)
        {
            particle2.Stop();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<IDamageable>();
        if (player != null)
        {
            player.GetDamage();
        }
    }
    public void Activate()
    {
        TurnOn();
        // Debug.Log("Activate");
        // if(!_on) TurnOn();
        // else TurnOff();
    }

    public void Desactivate()
    {
        TurnOff();
        // Debug.Log("Desactivate");
        // if(_on) TurnOff();
        // else TurnOn();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(_particles2[0].transform.position, -transform.up * distance);
    }
}
