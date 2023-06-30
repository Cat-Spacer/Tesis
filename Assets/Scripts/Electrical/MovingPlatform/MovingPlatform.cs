using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class MovingPlatform : MonoBehaviour, IElectric, IMouseOver
{
    Action _MoveAction = delegate { };
    [SerializeField] private Vector3 _offset;
    public GameObject platform;
    [Header("Waypoints")]
    [SerializeField] private List<Transform> _waypoitns;
    public int currentWaypoint = 0;
    [Header("Data")]
    [SerializeField] private float _speed = 0, _baseSpeed = 3;
    public float arriveMag = 0.5f;
    public int maxLenght = 0;
    [SerializeField] private bool _forceTurnOn = false;
    [SerializeField] private GameObject pivot, _connectionSource;
    [SerializeField] private Sprite _turnOnSprite, _turnOffSprite;
    [SerializeField] private bool _alwaysOn = false;
    [SerializeField] private SoundManager.Types _sound = SoundManager.Types.Platform;
    private bool stop = false;
    private IGenerator _myGen;
    private Vector3 _startPos;
    private SpriteRenderer _sp;
    private Rigidbody2D _myRB2D;
    private LineRenderer _myLineConnection;
    private int _senseDir = 1, startingWaypoint;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _startPos = platform.transform.position;
        startingWaypoint = currentWaypoint;
        EventManager.Instance.Subscribe("PlayerDeath", ResetPosition);
        maxLenght = _waypoitns.Count - 1;
        _speed = _baseSpeed;
        _myRB2D = platform.GetComponent<Rigidbody2D>();
        if (_forceTurnOn) TurnOn();
        else TurnOff();
    }

    private void FixedUpdate()
    {
        _MoveAction();
    }
    void Movement()
    {
        float dist = Vector3.Distance(pivot.transform.position, _waypoitns[currentWaypoint].position);
        Vector3 dir = _waypoitns[currentWaypoint].position - (platform.transform.position + _offset);
        dir.Normalize();
        if (dist < 0.1)
        {
            if (currentWaypoint == maxLenght)
            {
                if (_senseDir == 1)
                {
                    _senseDir = -1;
                    maxLenght = 0;
                }
                else
                {
                    _senseDir = 1;
                    maxLenght = _waypoitns.Count - 1;
                }
            }
            currentWaypoint += _senseDir;
        }
        if (!stop)
        {
            if (dist < arriveMag)
            {
                _myRB2D.velocity = dir * _speed * dist * Time.fixedDeltaTime;
            }
            else
            {
                _myRB2D.velocity = dir * _speed * Time.fixedDeltaTime;
            }
        }
        else if (stop)
        { 
            _myRB2D.velocity = new Vector2(0, 0); 
        }       
    }

    public void TurnOn()
    {
        SoundManager.instance.Play(_sound);
        _MoveAction = Movement;
        _sp.sprite = _turnOnSprite;
    }

    public void TurnOff()
    {
        SoundManager.instance.Pause(_sound);
        _MoveAction = delegate { };
        _sp.sprite = _turnOffSprite;
        _myRB2D.velocity = new Vector2(0, 0);
    }

    public Transform ConnectionSource()
    {
        return _connectionSource.transform;
    }

    public void SetGenerator(IGenerator gen, LineRenderer line)
    {
        _myGen = gen;
        _myLineConnection = line;
    }

    public void SetGenerator(LineRenderer line)
    {
        _myLineConnection = line;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 27)
        {
            stop = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 27)
            stop = false;
    }

    void ResetPosition(params object[] param)
    {
        if (_alwaysOn) return;
        TurnOff();
        platform.transform.position = _startPos;
        currentWaypoint = startingWaypoint;
    }

    public void MouseOver()
    {
        if (_myGen == null) return;
        _myGen.ShowLineConnection(_myLineConnection);
    }

    public void MouseExit()
    {
        if (_myGen == null) return;
        _myGen.NotShowLineConnection(_myLineConnection);
    }

    public void Interact()
    {

    }
}