using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private SpriteRenderer _sp;
    [SerializeField] private SpriteRenderer[] _bloodStun;
    int dmgTimes = 0;
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        foreach (var item in _bloodStun)
        {
            item.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
        if (_bloodStun != null && _bloodStun.Length > dmgTimes)
            _bloodStun[dmgTimes].enabled = true;
        dmgTimes += 1;
        obj.GetDamage();
    }
}