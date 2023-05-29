using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField]private float _damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
        obj.GetDamage();
    }
}
