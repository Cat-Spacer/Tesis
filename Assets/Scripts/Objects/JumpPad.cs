using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JumpPad : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private bool _isOn;
    private LineRenderer _myLineConnection;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        if (_isOn)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOff()
    {
        //_particle.Stop();
        _isOn = false;
    }

    public void TurnOn()
    {
        //_particle.Play();
        _isOn = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isOn) return;
        if ((_mask.value & (1 << col.transform.gameObject.layer)) > 0)
        {
            SoundManager.instance.Play(SoundsTypes.CatDash);
            var player = col.gameObject.GetComponent<PlayerCharacter>();
            player.StopMovement();
            var entityRb = col.gameObject.GetComponent<Rigidbody2D>();
            entityRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            _anim.SetTrigger("Activate");
            //SoundManager.instance.Play("Mushroom");
        }
    }

}