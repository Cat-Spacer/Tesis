using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectToSpawn
{
    [Header("Stats")]
    [Header("Has ObjectToSpawn on it")]
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
        Debug.Log($"{name} colisiono con {collision.name}");
        ObjectFactory objectFactory = FindObjectOfType<ObjectFactory>();
        objectFactory.ReturnObject(this);
    }
}
