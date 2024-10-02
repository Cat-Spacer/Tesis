using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private SpriteRenderer _sp;
    private BoxCollider2D _col;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _col = GetComponent<BoxCollider2D>();

        _col.size = new Vector2(_sp.size.x, .7f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
        obj.GetDamage();
    }
}