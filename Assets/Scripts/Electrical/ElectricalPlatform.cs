using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPlatform : MonoBehaviour, IElectric
{
    [SerializeField] Transform _PointATop;
    [SerializeField] Transform _PointBLow;
    Vector3 _newPos;
    [SerializeField] float _speed;
    [SerializeField] GameObject _canvas;

    [SerializeField] bool _turnOn = false;

    void Awake()
    {
        _newPos = transform.position;
        if (_turnOn) { _canvas.gameObject.SetActive(true); }
        else { _canvas.gameObject.SetActive(false);}
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
        if (_turnOn && transform.position.y <= _PointATop.position.y && transform.position.y >= _PointBLow.position.y)
        {
            if (_newPos.y <= _PointATop.position.y && _newPos.y >= _PointBLow.position.y)
            {
                Vector3 newYPos = new Vector3(transform.position.x, _newPos.y, transform.position.z);

                transform.position = Vector3.MoveTowards(transform.position, newYPos, _speed * Time.deltaTime);
            }
        }
    }
    public void UpArrow()
    {
        _newPos.y += 3;
        if (_newPos.y > _PointATop.position.y)
            _newPos.y = _PointATop.position.y;
    }
    public void DownArrow()
    {
        _newPos.y -= 3;
        if (_newPos.y < _PointBLow.position.y)
            _newPos.y = _PointBLow.position.y;
    }
}
