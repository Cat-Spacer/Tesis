using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxLife;
    [SerializeField] private float _currentLife;
    public float dmg;
    private void Start()
    {
        _currentLife = _maxLife;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
        obj.GetDamage(dmg);
    }

    public void GetDamage(float dmg)
    {
        _currentLife -= dmg;
        if (_currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}
