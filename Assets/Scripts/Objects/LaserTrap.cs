using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour, IActivate
{
    [SerializeField] LineRenderer _line;
    [SerializeField] private Transform linePoint;
    [SerializeField] LayerMask _hitLayerMask;
    [SerializeField] ParticleSystem[] _particles;
    [SerializeField] ParticleSystem[] _particles2;
    [SerializeField] float _loopTime;
    [SerializeField] bool _loop;
    bool _firstStart;
    [SerializeField] bool _on;
    [SerializeField] bool start;
    BoxCollider2D coll;
    private LineRenderer _myLineConnection;
    void Start()
    {
        _firstStart = true;
        coll = GetComponent<BoxCollider2D>();
        if(start) TurnOn();
        else TurnOff();
        // if (_on)
        // {
        //     TurnOn();
        // }
        // else
        // {
        //     TurnOff();
        // }
    }

    void TurnOn()
    {
        _on = true;
        _line.enabled = true;
        _firstStart = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, 10, _hitLayerMask);
        if (hit)
        {
            foreach (var particle2 in _particles2)
            {
                particle2.Play();
            }
            _line.SetPosition(0, linePoint.position);
            _line.SetPosition(1, hit.point);
            float dist = Vector2.Distance(transform.position, hit.point);
            var center = dist / 2;
            coll.offset = new Vector2(0, center);
            coll.size = new Vector2(0.1f, dist);
            foreach (var particle in _particles)
            {
                particle.transform.position = hit.point;
                particle.Play();
            }
            if (_loop)
            {
                StartCoroutine(LoopTurnOff());
            }
        }
    }
    void TurnOff()
    {
        _firstStart = false;
        _on = false;
        _line.enabled = false;
        foreach (var particle in _particles)
        {
            particle.Stop();
        }
        foreach (var particle2 in _particles2)
        {
            particle2.Stop();
        }
        if (_loop)
        {
            StartCoroutine(LoopTurnOn());
        }
    }
    IEnumerator LoopTurnOn()
    {
        yield return new WaitForSeconds(_loopTime);
        TurnOn();
    }
    IEnumerator LoopTurnOff()
    {
        yield return new WaitForSeconds(_loopTime);
        TurnOff();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<IDamageable>();
        if (player != null && _on)
        {
            player.GetDamage();
        }
    }
    public void Activate()
    {
        Debug.Log("Activate");
        TurnOn();
    }

    public void Desactivate()
    {
        Debug.Log("Desactivate");
        TurnOff();
    }
}
