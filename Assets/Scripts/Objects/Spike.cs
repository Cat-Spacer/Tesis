using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj == null) return;
        obj.GetDamage();
    }
}
