using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniMenu : MonoBehaviour
{
    private Action _OpenCloseAction = delegate { };
    [SerializeField] Transform _openPos;
    [SerializeField] Transform _closePos;
    [SerializeField] float _speed;
    [SerializeField] float _timeUntilClose;
    float _timeUntilCloseDefault;

    private void Start()
    {
        _timeUntilCloseDefault = _timeUntilClose;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _OpenCloseAction = Open;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _OpenCloseAction = Close;
        }
        _OpenCloseAction();
    }
    public void OpenCall()
    {
        Debug.Log("LLamado");
        _timeUntilClose = _timeUntilCloseDefault;
        _OpenCloseAction = Open;
        _OpenCloseAction += Counter;
    }
    private void Open()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
        if (transform.position.x >= _openPos.position.x)
        {
            _OpenCloseAction -= Open;
            _OpenCloseAction += delegate { };
            transform.position = _openPos.position;
        }
    }
    public void CloseCall()
    {      
        _OpenCloseAction = Close;        
    }
    private void Close()
    {
        transform.position += transform.right * -_speed * Time.deltaTime;      
        if (transform.position.x <= _closePos.position.x)
        {
            _OpenCloseAction -= Close;
            _OpenCloseAction += delegate { };
            transform.position = _closePos.position;
        }
    }
    private void Counter()
    {
        _timeUntilClose -= Time.deltaTime;
        if (_timeUntilClose <= 0)
        {
            _OpenCloseAction = Close;
            _timeUntilClose = _timeUntilCloseDefault;
        }
    }
}