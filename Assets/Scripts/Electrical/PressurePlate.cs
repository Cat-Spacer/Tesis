using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float speedUp, speedDown;
    [SerializeField] Transform _topPoint, _downPoint;
    [SerializeField] private GameObject _plate;
    [SerializeField] private GameObject _connectionObj;
    private IActivate _connection;
    void Start()
    {
        _connection = _connectionObj.GetComponent<IActivate>();
    }
    void Activate()
    {
        transform.position = _downPoint.position;
        _connection.Activate();
    }

    void Desactivate()
    {
        transform.position = _topPoint.position;
        _connection.Desactivate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            Desactivate();
        }
    }
}
