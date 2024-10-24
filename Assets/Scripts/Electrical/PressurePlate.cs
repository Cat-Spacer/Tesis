using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Transform _topPoint, _downPoint;
    [SerializeField] private GameObject _plate;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection  = new List<IActivate>();
    void Start()
    {
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            Debug.Log(obj);
            _connection.Add(obj);
        }
    }
    void Activate()
    {
        _plate.transform.position = _downPoint.position;
        foreach (var connection in _connection)
        {
            connection.Activate();
        }
    }

    void Desactivate()
    {
        _plate.transform.position = _topPoint.position;
        foreach (var connection in _connection)
        {
            connection.Desactivate();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<CatCharacter>();
        if (player != null)
        {
            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<CatCharacter>();
        if (player != null)
        {
            Desactivate();
        }
    }
}
