using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    [SerializeField] private float _maxLife;
    [SerializeField] private float _currentLife;
    [SerializeField] ParticleSystem[] particles;  
    public float dmg;

    private void Start()
    {
        _currentLife = _maxLife;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
        obj.GetDamage();
    }
}
