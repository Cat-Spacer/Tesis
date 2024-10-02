using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FallingTrapNetwork : NetworkBehaviour, IActivate
{
    private Action ActivateAction = delegate {  };
    [SerializeField] private FallingTrapBulletNetwork currentBullet;
    [SerializeField] private LayerMask rayPlayerMask;
    [SerializeField] private LayerMask rayGroundMask;
    
    [SerializeField] private bool activated;
    [SerializeField] private bool readyToShoot;
    [SerializeField] private float cdTime;
    [SerializeField] private Transform[] raysPos;
    [SerializeField] private Transform bulletPos;

    private float rayLenght;
    void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10, rayGroundMask);
        if (hit)
        {
            rayLenght = hit.distance;
        }
        currentBullet.gameObject.SetActive(false);
        if (NetworkManager!= null && NetworkManager.ServerIsHost)
        {
            if(activated) RechargeBullet();
        }
    }
    void Update()
    {
        ActivateAction();
    }

    private void RechargeBullet()
    {
        currentBullet.gameObject.SetActive(true);
        ActivateAction = ChequeForPlayer;
    }
    private void ChequeForPlayer()
    {
        foreach (var rayPos in raysPos)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayPos.transform.position, Vector2.down, rayLenght, rayPlayerMask);
            if (hit && IsReady())
            {
                ActivateAction = Shoot;
            }
        }
    }

    private void Shoot()
    {
        currentBullet.Activate();
        
        ActivateAction = delegate {  };
        StartCoroutine(Cooldown());
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(cdTime);
        RechargeBullet();
    }

    public bool IsReady()
    {
        return currentBullet.ready;
    }
    
    
    public void Activate()
    {
        TurnOnRpc();
    }
    [Rpc(SendTo.Server)]
    void TurnOnRpc()
    {
        ActivateAction = ChequeForPlayer;
    }
    public void Desactivate()
    {
        TurnOffRpc();
    }
    [Rpc(SendTo.Server)]
    void TurnOffRpc()
    {
        ActivateAction = delegate {  };
    }
}
