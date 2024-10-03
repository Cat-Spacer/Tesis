using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class CrushinTrap : MonoBehaviour, IActivate
{
    private Animator _anim;

    [SerializeField] private bool startingState;
    [SerializeField] private float smashSpeed;
    [SerializeField] private float recoverSpeed;
    [SerializeField] private float startDelay;
    [SerializeField] private float stayDelay;
    [SerializeField] private float finishDelay;
    private static readonly int SmashState = Animator.StringToHash("Smash");
    private static readonly int ReturnState = Animator.StringToHash("Return");

    private bool isOn;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        if(startingState)
        {
            isOn = true;
            Activate();
        }
    }
    

    IEnumerator Smash()
    {
        yield return new WaitForSecondsRealtime(startDelay);
        if (isOn)
        {
            _anim.speed = smashSpeed;
            _anim.SetTrigger(SmashState);
        }
    }

    public IEnumerator SmashStay()
    {
        yield return new WaitForSecondsRealtime(stayDelay);
        _anim.speed = recoverSpeed;
        _anim.SetTrigger(ReturnState);
    }
    public IEnumerator Return()
    {
        yield return new WaitForSecondsRealtime(finishDelay);
        StartCoroutine(Smash());
    }
    public void Activate()
    {
        isOn = true;
        _anim.speed = smashSpeed;
        StartCoroutine(Smash());
    }

    public void Desactivate()
    {
        isOn = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.GetDamage();
        }
    }
}
