using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class DoorNetwork : NetworkBehaviour, IActivate
{
    private BoxCollider2D _coll;
    [SerializeField] private bool _activated;
    private NetworkVariable<bool> _OnOpen = new NetworkVariable<bool>();
    private SpriteRenderer _sp;
    [SerializeField] private Color activatedColor, desactivatedColor;

    [SerializeField] private float speedChange;
    
    private void Awake()
    {
        _coll = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        if (!_activated)
        {
            _sp.color = desactivatedColor;
            _coll.enabled = false;
            _activated = false;
        }
        else
        {
            _sp.color = activatedColor;
            _coll.enabled = true;
            _activated = true;
        }
    }
    void OpenClose()
    {
        if (!IsOwner) return;
        if (!_activated) ActivateRpc();
        else DesactivateRpc();
    }
    public void Activate()
    {
        OpenClose();
    }

    [Rpc(SendTo.Everyone)]
    void ActivateRpc()
    {
        _sp.color = activatedColor;
        _coll.enabled = true;
        _activated = true;
    }
    public void Desactivate()
    {
        OpenClose();
    }
    [Rpc(SendTo.Everyone)]
    void DesactivateRpc()
    {
        _sp.color = desactivatedColor;
        _coll.enabled = false;
        _activated = false;
    }
    
}