using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectToSpawn
{
    [Header("Stats")]
    [Header("Has ObjectToSpawn on it")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] private float _speed = 1.0f;

    void Update()
    {
        NormalMovement();
    }

    private void NormalMovement()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        var obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null)
        {
            obj.GetDamage(10);            
        }
        if ((enemyMask.value & (1 << collision.transform.gameObject.layer)) > 0) return;
        TurnOff(this);
        ObjectFactory objectFactory = FindObjectOfType<ObjectFactory>();
        objectFactory.ReturnObject(this);
    }

}
