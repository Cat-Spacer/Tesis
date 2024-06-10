using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorNetwork : NetworkBehaviour, IActivate
{
    private BoxCollider2D _coll;
    [SerializeField] private bool _open;

    private void Awake()
    {
        _coll = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {
        if (_open)
        {
            gameObject.SetActive(false);
            _coll.gameObject.SetActive(false);
            _open = true;
        }
        else
        {
            gameObject.SetActive(true);
            _coll.gameObject.SetActive(true);
            _open = false;
        }
    }

    void OpenClose()
    {
        if (!_open)
        {
            //gameObject.SetActive(false);
            //_coll.gameObject.SetActive(false);
            //_open = true;
            ActivateRpc();
        }
        else
        {
            //gameObject.SetActive(true);
            //_coll.gameObject.SetActive(true);
            //_open = false;
            DesactivateRpc();
        }
    }
    public void Activate()
    {
        OpenClose();
    }

    [Rpc(SendTo.Everyone)]
    void ActivateRpc()
    {
        gameObject.SetActive(false);
        _coll.gameObject.SetActive(false);
        _open = true;
    }
    public void Desactivate()
    {
        OpenClose();
    }
    [Rpc(SendTo.Everyone)]
    void DesactivateRpc()
    {
        gameObject.SetActive(true);
        _coll.gameObject.SetActive(true);
        _open = false;
    }
    
}