using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour, IElectric
{
    [SerializeField] LineRenderer _line;
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] ParticleSystem[] _particles;
    [SerializeField] ParticleSystem[] _particles2;
    [SerializeField] float _loopTime;
    [SerializeField] bool _loop;
    bool _firstStart;
    [SerializeField] bool _on;

    void Start()
    {
        _firstStart = true;
        if (_on)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOn()
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10, _groundLayerMask);
                if (hit)
                {
                    foreach (var particle2 in _particles2)
                    {
                        particle2.Play();
                    }
                    _line.SetPosition(0, transform.position);
                    _line.SetPosition(1, hit.point);
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
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.up, 10, _groundLayerMask);
            if (hit2)
            {
                foreach (var particle2 in _particles2)
                {
                    particle2.Play();
                }
                _line.SetPosition(0, transform.position);
                _line.SetPosition(1, hit2.point);
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
    public void TurnOff()
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
}
