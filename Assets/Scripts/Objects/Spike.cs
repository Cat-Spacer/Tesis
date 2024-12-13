using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private SpriteRenderer _sp;
    private BoxCollider2D _col;
    [SerializeField] private SpriteRenderer[] _bloodStun;
    int dmgTimes = 0;
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _col = GetComponent<BoxCollider2D>();

        _col.size = new Vector2(_sp.size.x, .7f);
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