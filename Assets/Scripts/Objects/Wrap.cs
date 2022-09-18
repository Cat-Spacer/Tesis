using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wrap : MonoBehaviour
{
    //[SerializeField] private float _time = 3.0f;
    //[SerializeField] private float _timer = 3.0f;
    [SerializeField] private float _speedToTramp = 10.0f;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private CustomMovement _player;


    Action<float> _onCatch = delegate { };

    private void Awake()
    {
        if (_myAnimator == null)
            _myAnimator = GetComponent<Animator>();
        if (_myAnimator == null)
            Debug.LogWarning($"No animator added to {name}.");

        if (_player == null)
            _player = FindObjectOfType<CustomMovement>();
        if (_player == null)
            Debug.LogWarning($"No player on scene.");

        //_time = _timer;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"Entre y triggerie con {collision.name}");
        if (collision.GetComponent<CustomMovement>() == _player)
        {
            _player.rb.AddForceAtPosition(_player.faceDirection * Vector2.right * _speedToTramp, Vector2.zero);
            //Animation play catch

            if (Vector2.Distance(_player.transform.position, transform.position) <= 1)
            {
                Debug.Log($"{collision.name} murio");
                var playerDamage = collision.gameObject.GetComponent<IDamageable>();
                playerDamage.GetDamage(1);//Animation play kill
            }

        }
    }
}
