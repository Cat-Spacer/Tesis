using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wrap : MonoBehaviour, ILiberate
{ 
    // the mouth is the center
    private float _time = 0;
    [SerializeField] private float _timer = 0;
    [SerializeField] private float _speedToTramp = 0;
    [SerializeField] private float _heithSize = 0;
    [SerializeField] private float _wrapSpeed = 0;
    [SerializeField] private float _trapLife = 0;
    private float _maxTrapLife = 0;
    [SerializeField] private bool _up = true, _onTrap = false;
    [SerializeField] private int _relaseCuant = 20;
    [SerializeField] private Transform _playerTrapPoint;
    [SerializeField] private GameObject _mouth, _wraps;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private CustomMovement _player;
    [SerializeField] private BoxCollider2D _myboxCollider;
    [SerializeField] private Transform _liberatedPos;
    private ITrap playerITrap;
    private float _wrpHeight = 0.0f;
    private Vector2 _wrpIntPos;

    Action<float> _onCatch = delegate { };

    private void Awake()
    {
        if (_myboxCollider == null)
            _myboxCollider = GetComponent<BoxCollider2D>();
        if (_myboxCollider == null)
            Debug.LogWarning($"No box collider added to {name}.");

        if (_player == null)
            _player = FindObjectOfType<CustomMovement>();
        if (_player == null)
            Debug.LogWarning($"No player on scene.");

        _time = _wrpHeight = _timer = _heithSize;
        //_myboxCollider.offset = new Vector2(0, -_heithSize / 2.0f);
        _wrpIntPos = _wraps.transform.position;
        _maxTrapLife = _trapLife;
    }

    private void Update()
    {
        //open / close trap
        //_myboxCollider.offset = new Vector2(0, _wrpHeight - (_heithSize / 2.0f + _time));
        if (_timer > 1.0f && _up)
        {
            _wrpHeight += Time.deltaTime;
            //var sp = _wraps.GetComponent<SpriteRenderer>();
            //sp.size = new Vector2(.5f, _timer);
            _wraps.transform.localScale = new Vector2(.5f, _timer);
            _timer -= Time.deltaTime * _wrapSpeed;
        }
        else if (_timer <= _time)
        {
            _up = false;
            _wrpHeight -= Time.deltaTime;
            //var sp = _wraps.GetComponent<SpriteRenderer>();
            //sp.size = new Vector2(.5f, _timer);
            _wraps.transform.localScale = new Vector2(.5f, _timer);
            _timer += Time.deltaTime * _wrapSpeed;
        }
        else
        {
            _timer = _time;
            _up = true;
        }
    }

    private void FixedUpdate()
    {
        //_wraps.transform.position = new Vector2(_wrpIntPos.x, _wrpHeight + (_wrpIntPos.y - _time));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("call ForceDashEnd");
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            _up = true;
            _onTrap = true; 
            _myAnimator.SetBool("Open", true);
            SoundManager.instance.Play(SoundManager.Types.CarnivorousPlant);
            _player.ForceDashEnd();
            _player.transform.position = _playerTrapPoint.position;
            if (playerITrap == null)
            {
                playerITrap = _player.GetComponent<ITrap>();
            }           
            playerITrap.Trap(true, gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"Entre y triggerie con {collision.name}");
        if (collision.GetComponent<CustomMovement>() == _player && _onTrap)
        {
            //_player.rb.velocity = new Vector2(0, _speedToTramp);
            //Animation play catching
            _player.transform.position = _playerTrapPoint.position;
            if (Vector2.Distance(_player.transform.position, transform.position) <= 1)
            {
                Debug.Log($"{collision.name} murio");
                var playerDamage = collision.gameObject.GetComponent<IDamageable>();
                playerDamage.GetDamage(1);//Animation play kill
            }           
        }
    }

    public void TryLiberate()
    {
        _trapLife -= 1;
        if (_trapLife <= 0)
        {
            var trap = _player.GetComponent<ITrap>();
            if (trap == null) return;
            trap.Trap(false, gameObject);
            _trapLife = _maxTrapLife;
            _onTrap = false;
        }
    }
}
