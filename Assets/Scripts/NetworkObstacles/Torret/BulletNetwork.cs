using System;
using System.Collections;
using System.Collections.Generic;
using Netcode.Extensions;
using Unity.Netcode;
using UnityEngine;

public class BulletNetwork : NetworkBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _speed = 1.0f, _lifeTime;
    GameObject _myFahter = default;
    public GameObject _prefab;

    void Update()
    {
        if(ServerIsHost) NormalMovement();
    }

    private void NormalMovement()
    {
        transform.position += transform.right * (_speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != _myFahter)
        {
            var obj = collision.gameObject.GetComponent<IDamageable>();
            if (obj != null) obj.GetDamage();
            
            gameObject.SetActive(false);
            if(NetworkManager.Singleton.IsServer) 
            {
                NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, _prefab);
                NetworkObject.Despawn(false);
            }
        }
    }
    public void SetBullet(Transform father, float speed, float lifeTime, GameObject prefab)
    {
        _myFahter = father.gameObject;
        //_speed = speed;
        _prefab = prefab;
    }
}
