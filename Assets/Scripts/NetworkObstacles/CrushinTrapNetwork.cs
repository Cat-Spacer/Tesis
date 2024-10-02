using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using Unity.Netcode;
using UnityEngine;

public class CrushinTrapNetwork : NetworkBehaviour, IActivate
{
    private Animator _anim;

    [SerializeField] private bool _startingState;
    [SerializeField] private float _speed;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _finishDelay;
    private static readonly int Smash1 = Animator.StringToHash("Smash");

    private bool isOn;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = _speed;
        if(_startingState)
        {
            isOn = true;
            StartCoroutine(Smash());
        };
    }
    

    IEnumerator Smash()
    {
        yield return new WaitForSecondsRealtime(_startDelay);
        if (isOn)
        {
            _anim.SetTrigger(Smash1);
        }
    }
    public IEnumerator Return()
    {
        yield return new WaitForSecondsRealtime(_finishDelay);
        StartCoroutine(Smash());
    }
    public void Activate()
    {
        TurnOnRpc();
    }

    [Rpc(SendTo.Everyone)]
    void TurnOnRpc()
    {
        isOn = true;
        StartCoroutine(Smash());
    }
    public void Desactivate()
    {
        TurnOffRpc();
    }
    [Rpc(SendTo.Everyone)]
    void TurnOffRpc()
    {
        isOn = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<PlayerCharacterMultiplayer>();
        if (player != null)
        {
            player.GetDamage();
        }
    }
}
