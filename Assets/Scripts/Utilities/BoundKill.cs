using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCharacter>() != null)
        {
            var damageable = other.GetComponent<IDamageable>();
            if(damageable == null) return;
            damageable.GetDamage();
        }
    }
}
