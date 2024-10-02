using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrapBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float fallSpeed;
    public bool ready;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }
    public void Activate()
    {
        if (!ready) return;
        _rb.gravityScale = fallSpeed;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.GetDamage();
        }
        Destroy(gameObject);
    }
}
