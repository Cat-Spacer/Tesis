using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wrap : MonoBehaviour, ILiberate
{
    // the mouth is the center
    Action _TrapLifeAction = delegate { };
    private float _time = 0;
    [SerializeField] float _timer = 0;
    [SerializeField] float _speedToTramp = 0;
    [SerializeField] float _heithSize = 0;
    [SerializeField] float _wrapSpeed = 0;
    [SerializeField] float _trapLife = 0;
    [SerializeField] float _trapLifeRecover = 0;
    private float _maxTrapLife = 0;
    [SerializeField] bool _up = true, _onTrap = false;
    [SerializeField] int _relaseCuant = 20;
    [SerializeField] Transform _playerTrapPoint;
    [SerializeField] GameObject _mouth, _wraps, _toung;
    [SerializeField] Animator _myAnimator;
    [SerializeField] CustomMovement _player;
    [SerializeField] BoxCollider2D _myboxCollider;
    [SerializeField] Transform _liberatedPos;
    [SerializeField] SpriteRenderer sp;
    [SerializeField] CatCanvas catCanvas;
    [SerializeField] ParticleSystem spitleParticle;
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
            var sp = _wraps.GetComponent<SpriteRenderer>();
            sp.size = new Vector2(2.06f, _timer);
            _myboxCollider.size = new Vector2(2.06f, _timer);
            _myboxCollider.offset = new Vector2(_myboxCollider.offset.x, -_timer * 0.5f);
            _toung.transform.position = new Vector2(_toung.transform.position.x, sp.bounds.min.y);
            _playerTrapPoint.position = new Vector2(_toung.transform.position.x, sp.bounds.min.y);
            _timer -= Time.deltaTime * _wrapSpeed;
        }
        else if (_timer <= _time)
        {
            _up = false;
            _wrpHeight -= Time.deltaTime;
            var sp = _wraps.GetComponent<SpriteRenderer>();
            sp.size = new Vector2(2.06f, _timer);
            _myboxCollider.size = new Vector2(2.06f, _timer);
            _myboxCollider.offset = new Vector2(_myboxCollider.offset.x, -_timer * 0.5f);
            _toung.transform.position = new Vector2(_toung.transform.position.x, sp.bounds.min.y);
            _playerTrapPoint.position = new Vector2(_toung.transform.position.x, sp.bounds.min.y);
            _timer += Time.deltaTime * _wrapSpeed;
        }
        else
        {
            _timer = _time;
            _up = true;
        }
        _TrapLifeAction();
    }

    private void FixedUpdate()
    {
        //_wraps.transform.position = new Vector2(_wrpIntPos.x, _wrpHeight + (_wrpIntPos.y - _time));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            _up = true;
            _onTrap = true; 
            _myAnimator.SetTrigger("Open");
            SoundManager.instance.Play(SoundManager.Types.CarnivorousPlant);
            _player.ForceDashEnd();
            _player.transform.position = _playerTrapPoint.position;
            if (playerITrap == null)
            {
                playerITrap = _player.GetComponent<ITrap>();
            }           
            playerITrap.Trap(true, _trapLife, gameObject);
            catCanvas = collision.gameObject.GetComponentInChildren<CatCanvas>();      
            _TrapLifeAction = TrapLifeRecover;
            spitleParticle.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<CustomMovement>();
        if (player == _player && _onTrap)
        {
            _player.transform.position = _playerTrapPoint.position;
            //catCanvas.TrapLifeUpdate(_trapLife);
            if (Vector2.Distance(_player.transform.position, transform.position) <= 1)
            {
                var playerDamage = collision.gameObject.GetComponent<IDamageable>();
                playerDamage.GetDamage();//Animation play kill
                _TrapLifeAction = delegate { };
                spitleParticle.gameObject.SetActive(false);
                _myAnimator.SetTrigger("Eat");
            }           
        }
    }

    private void TrapLifeRecover()
    {
        _trapLife += _trapLifeRecover * Time.deltaTime;
    }

    public void TryLiberate()
    {
        _trapLife -= 1;
        if (_trapLife <= 0)
        {
            var trap = _player.GetComponent<ITrap>();
            if (trap == null) return;
            trap.Trap(false, _trapLife,gameObject);
            _trapLife = _maxTrapLife;
            _onTrap = false;
            _TrapLifeAction = delegate { };
            _myAnimator.SetTrigger("Close");
            spitleParticle.gameObject.SetActive(false);
        }
    }
}
