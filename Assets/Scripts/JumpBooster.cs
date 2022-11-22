using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBooster : MonoBehaviour
{
    [SerializeField] CustomMovement _player;

    [SerializeField] float boostForce;


    private void Start()
    {
        _player = FindObjectOfType<CustomMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(CustomMovement.isDashing);

        if (collision.gameObject.layer == _player.gameObject.layer)
        {
            _player.BoostJump(boostForce);

        }
    }
}
