using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour, IElectric
{
    [SerializeField] LineRenderer _line;
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] ParticleSystem[] _particles;

    void Start()
    {
        TurnOn();
    }

    public void TurnOn()
    {
        _line.enabled = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 10, _groundLayerMask);
        Debug.Log(transform.position - Vector3.down);
        if (hit)
        {
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, hit.point);
            foreach (var particle in _particles)
            {
                particle.transform.position = hit.point;
                particle.Play();
            }
        }
    }
    public void TurnOff()
    {
        _line.enabled = false;
        foreach (var particle in _particles)
        {
            particle.Stop();
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
}
