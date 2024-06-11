using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Torret : NetworkBehaviour, IActivate
{
    private Action _shootAction = delegate { };
    [Header("Stats")]
    public int damage = 10;
    public float fireRate = 1.0f, fireTimer = 1.0f, bulletLifeTime = 1.0f, _waitUntilShoot = 1.0f, distance = 150f;

    [Header("Objects")]
    [SerializeField] private  Transform _firePoint = default;
    [SerializeField] private GameObject _bulletPrefab;
    private bool _isActive = false;
    
    private void Awake()
    {
        fireTimer = fireRate;
    }
    
    private void Start()
    {
        if (_isActive) _shootAction = FireCooldown;
    }
    
    private void Update()
    {
        _shootAction();
        FireCooldown();
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
            StartCoroutine(WaitForAnim());
        }
    }
    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(_waitUntilShoot);
        Fire();
    }

    private void Fire()
    {
        if (IsServer)
        {
            var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            bullet.GetComponent<NetworkObject>().Spawn();
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _firePoint.rotation;
        }
        else FireRpc();
    }
    [Rpc(SendTo.Server)]
    private void FireRpc()
    {
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn();
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = _firePoint.rotation;
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
