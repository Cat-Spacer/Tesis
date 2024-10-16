using System;
using System.Collections;
using Netcode.Extensions;
using UnityEngine;
using Weapons;

public class TorretNetwork : Gun, IActivate
{
    private Action _shootAction = delegate { };
    [Header("Stats")]
    public int damage = 10;

    public float fireRate = 1.0f, fireTimer = 1.0f, bulletLifeTime = 1.0f, _waitUntilShoot = 1.0f, _bulletSpeed;

    [Header("Objects")]
    [SerializeField] private  Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] bool _isActive;
    
    private void Awake()
    {
        fireTimer = fireRate;
    }
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        _shootAction();
        //FireCooldown();
    }
    
    private void FireCooldown()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
        else
        {
            fireTimer = fireRate;
            Fire();
            //StartCoroutine(WaitForAnim());
        }
    }
    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(_waitUntilShoot); 
        Fire();
    }
 
    private void Fire()
    {
        var bullet = NetworkObjectPool.Singleton.GetNetworkObject(_bulletPrefab);
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = _firePoint.rotation;
        if(!bullet.IsSpawned) bullet.Spawn(true);
        bullet.GetComponent<BulletNetwork>().SetBullet(transform, _bulletSpeed, 5, _bulletPrefab);
    }

    public void Activate()
    {
        if(!_isActive) _shootAction = FireCooldown;
        else
        {
            _shootAction = delegate { };
            fireTimer = fireRate;
        }
    }

    public void Desactivate()
    {
        throw new System.NotImplementedException();
    }
}
