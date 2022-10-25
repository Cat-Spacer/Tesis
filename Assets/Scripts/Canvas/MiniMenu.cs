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
        _OpenCloseAction = Open;
        _OpenCloseAction += Counter;
    }
    private void Open()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
        var dist = Vector3.Distance(transform.position, _openPos.position);
        if (dist <= 0.1f)
        {
            _OpenCloseAction -= Open;
            _OpenCloseAction += delegate { };
        }
    }
    public void CloseCall()
    {      
        _OpenCloseAction = Close;        
    }
    private void Close()
    {
        transform.position += transform.right * -_speed * Time.deltaTime;
        var dist = Vector3.Distance(transform.position, _closePos.position);
        if (dist <= 0.1f)
        {
            _OpenCloseAction = delegate { };
        }
    }
    private void Counter()
    {
        _timeUntilClose -= Time.deltaTime;
        if (_timeUntilClose <= 0)
        {
            _OpenCloseAction = Close;
        }
    }
}
