using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingTrapBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float fallSpeed;
    public bool ready;
    [SerializeField] private Sprite[] trash;
    private SpriteRenderer sp;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        sp.sprite = trash[Random.Range(0, trash.Length - 1)];
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
