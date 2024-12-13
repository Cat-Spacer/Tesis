using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaserTrapNetwork : NetworkBehaviour, IActivate
{
    [SerializeField] LineRenderer _line;
    [SerializeField] LayerMask _hitLayerMask;
    [SerializeField] ParticleSystem[] _particles;
    [SerializeField] ParticleSystem[] _particles2;
    [SerializeField] float _loopTime;
    [SerializeField] bool _loop;
    bool _firstStart;
    [SerializeField] bool _on;
    BoxCollider2D coll;
    private LineRenderer _myLineConnection;
    void Start()
    {
        _firstStart = true;
        coll = GetComponent<BoxCollider2D>();
        if (_on)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    void TurnOn()
    {
        if (!_firstStart)
        {
            if (_on) //si ya esta prendido
            {
                Debug.Log("turnOff");
                //TurnOff();
            }
            else
            {
                _on = true;
                _line.enabled = true;
                _firstStart = false;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10, _hitLayerMask);
                if (hit)
                {
                    foreach (var particle2 in _particles2)
                    {
                        particle2.Play();
                    }
                    _line.SetPosition(0, transform.position);
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
        }
        else
        {
            _on = true;
            _line.enabled = true;
            _firstStart = false;
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.up, 10, _hitLayerMask);
            if (hit2)
            {
                foreach (var particle2 in _particles2)
                {
                    particle2.Play();
                }
                _line.SetPosition(0, transform.position);
                _line.SetPosition(1, hit2.point);
                float dist = Vector2.Distance(transform.position, hit2.point);
                var center = dist / 2;
                coll.offset = new Vector2(0, center);
                coll.size = new Vector2(0.1f, dist);
                foreach (var particle in _particles)
                {
                    particle.transform.position = hit2.point;
                    particle.Play();
                }

                if (_loop)
                {
                    StartCoroutine(LoopTurnOff());
                }
            }

        }
    }
    void TurnOff()
    {
        if (!_firstStart)
        {
            if (!_on) //si ya esta apagado
            {
                //TurnOn();
            }
            else
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
        }
        else
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

    public void Interact()
    {

    }

    public void Activate()
    {
        if (!IsOwner) return;
        ActivateOrDesactivateRpc();
    }
    public void Desactivate()
    {
        if (!IsOwner) return;
        ActivateOrDesactivateRpc();
    }
    [Rpc(SendTo.Everyone)]
    void ActivateOrDesactivateRpc()
    {
        if(!_on) TurnOn();
        else TurnOff();
    }
}
