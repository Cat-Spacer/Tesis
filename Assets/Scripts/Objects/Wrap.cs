using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wrap : MonoBehaviour
{
    // the mouth is the center
    [SerializeField] private float _time = 3.0f, _timer = 3.0f, _speedToTramp = 10.0f, _heithSize = 4.0f;
    [SerializeField] private bool _up = true;
    [SerializeField] private int _relaseCuant = 20;
    [SerializeField] private GameObject _mouth, _wraps;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private CustomMovement _player;
    [SerializeField] private BoxCollider2D _myboxCollider;
    [SerializeField] private Transform _liberatedPos;
    private float _wrpHeight = 0.0f;
    private Vector2 _wrpIntPos;

    Action<float> _onCatch = delegate { };

    private void Awake()
    {
        if (_myAnimator == null)
            _myAnimator = GetComponent<Animator>();
        if (_myAnimator == null)
            Debug.LogWarning($"No animator added to {name}.");

        if (_myboxCollider == null)
            _myboxCollider = GetComponent<BoxCollider2D>();
        if (_myboxCollider == null)
            Debug.LogWarning($"No box collider added to {name}.");

        if (_player == null)
            _player = FindObjectOfType<CustomMovement>();
        if (_player == null)
            Debug.LogWarning($"No player on scene.");

        _time = _wrpHeight = _timer = _heithSize;
        _myboxCollider.offset = new Vector2(0, -_heithSize / 2.0f);
        _wrpIntPos = _wraps.transform.position;
    }

    private void Update()
    {
        // open / close trap
        _myboxCollider.offset = new Vector2(0, _wrpHeight - (_heithSize / 2.0f + _time));
        if (_timer > 1.0f && _up)
        {
            _wrpHeight += Time.deltaTime;
            _timer -= Time.deltaTime;
        }
        else if (_timer <= _time)
        {
            _up = false;
            _wrpHeight -= Time.deltaTime;
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = _time;
            _up = true;
        }
    }

    private void FixedUpdate()
    {
        _wraps.transform.position = new Vector2(_wrpIntPos.x, _wrpHeight + (_wrpIntPos.y - _time));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("call ForceDashEnd");
        _player.ForceDashEnd();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"Entre y triggerie con {collision.name}");
        if (collision.GetComponent<CustomMovement>() == _player)
        {
            _player.rb.AddForceAtPosition(Vector2.up * _speedToTramp, _mouth.transform.position);// player being dragged to tramp
            //Animation play catching

            if (Vector2.Distance(_player.transform.position, transform.position) <= 1)
            {
                Debug.Log($"{collision.name} murio");
                var playerDamage = collision.gameObject.GetComponent<IDamageable>();
                playerDamage.GetDamage(1);//Animation play kill
            }
            else if (_relaseCuant < 0)
                _player.transform.position = _liberatedPos.position;
        }
    }

    public void PlayerLiberate()
    {
        _relaseCuant--;
    }
}
