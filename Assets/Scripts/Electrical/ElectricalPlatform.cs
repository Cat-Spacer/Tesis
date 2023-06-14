using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectricalPlatform : MonoBehaviour, IElectric
{
    [SerializeField] private Transform _PointATop;
    [SerializeField] private Transform _PointBLow;
    [SerializeField] private Transform[] _checkpoints;
    [SerializeField] int _current;
    [SerializeField] int _max;
    Transform _startPos;
    int _startCheckpoint;

    private Vector3 _newPos;
    [SerializeField] private float _speed, _moveScale = 3.0f;
    [SerializeField] private GameObject _canvas;

    [SerializeField] private bool _turnOn = false;

    [SerializeField] Vector2 _checkBottom;
    [SerializeField] Vector3 _offsetBottom;
    [SerializeField] LayerMask _playerLayerMask;

    Action _MoveAction = delegate { };

    void Awake()
    {
        _newPos = transform.position;
        if (_canvas)
        {
            if (_turnOn) { _canvas.gameObject.SetActive(true); }
            else { _canvas.gameObject.SetActive(false); }
        }

    }
    private void Start()
    {
        EventManager.Instance.Subscribe("PlayerDeath", ResetPlatform);
        _max = _checkpoints.Length - 1;
        _startCheckpoint = _current;
    }
    public void TurnOff()
    {
        _turnOn = false;
        _canvas.gameObject.SetActive(false);
    }

    public void TurnOn()
    {
        _turnOn = true;
        _canvas.gameObject.SetActive(true);
    }

    public void NewClickPosition(Transform pos_arg)
    {

    }

    void Update()
    {
        //if (_turnOn && transform.position.y <= _PointATop.position.y && transform.position.y >= _PointBLow.position.y)
        //{
        //    if (_newPos.y <= _PointATop.position.y && _newPos.y >= _PointBLow.position.y)
        //    {
        //        Vector3 newYPos = new Vector3(transform.position.x, _newPos.y, transform.position.z);

        //        transform.position = Vector3.MoveTowards(transform.position, newYPos, _speed * Time.deltaTime);
        //    }
        //}
        _MoveAction();
    }
    public void UpArrow()
    {
        //_newPos.y += _moveScale;
        //if (_newPos.y > _PointATop.position.y)
        //    _newPos.y = _PointATop.position.y;
        if (_current < _max)
        {
            _current++;
            _MoveAction = GoUp;
            _canvas.gameObject.SetActive(false);
        }
       
    }
    void GoUp()
    {
        //var dist = Vector2.Distance(transform.position, _PointATop.position);
        //if (dist > 0.05)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _PointATop.position, _speed * Time.deltaTime);
        //}
        //else
        //{
        //    if (_turnOn)
        //    {
        //        _canvas.gameObject.SetActive(true);
        //    }
        //    _MoveAction = delegate { };
        //}

        var dist = Vector2.Distance(transform.position, _checkpoints[_current].position);
        if (dist > 0.05)
        {
            transform.position = Vector3.MoveTowards(transform.position, _checkpoints[_current].position, _speed * Time.deltaTime);
        }
        else
        {
            if (_turnOn)
            {
                _canvas.gameObject.SetActive(true);
            }
            _MoveAction = delegate { };
        }
    }
    public void DownArrow()
    {
        //_newPos.y -= _moveScale;
        //if (_newPos.y < _PointBLow.position.y)
        //    _newPos.y = _PointBLow.position.y;
        if (_current > 0)
        {
            _current--;
            _MoveAction = GoDown;
            _canvas.gameObject.SetActive(false);
        }
    }
    void GoDown()
    {
        //if (Physics2D.OverlapBox(transform.position + _offsetBottom, _checkBottom, 0, _playerLayerMask)) return;
        //var dist = Vector2.Distance(transform.position, _PointBLow.position);
        //if (dist > 0.05)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _PointBLow.position, _speed * Time.deltaTime);
        //}
        //else
        //{
        //    if (_turnOn)
        //    {
        //        _canvas.gameObject.SetActive(true);
        //    }
        //    _MoveAction = delegate { };
        //}
        if (Physics2D.OverlapBox(transform.position + _offsetBottom, _checkBottom, 0, _playerLayerMask)) return;
        var dist = Vector2.Distance(transform.position, _checkpoints[_current].position);
        if (dist > 0.05)
        {
            transform.position = Vector3.MoveTowards(transform.position, _checkpoints[_current].position, _speed * Time.deltaTime);
        }
        else
        {
            if (_turnOn)
            {
                _canvas.gameObject.SetActive(true);
            }
            _MoveAction = delegate { };
        }
    }
    public void ResetPlatform(params object[] parameters)
    {
        transform.position = _startPos.position;
        _current = _startCheckpoint;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _offsetBottom, _checkBottom);
    }
}
