using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Unity.Netcode;

public class PressurePlateNetwork : NetworkBehaviour
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
            _connection.Add(obj);
        }
    }
    void Activate()
    {
        if (!IsOwner) return;
        ActivateRpc();
    }

    [Rpc(SendTo.Everyone)]
    void ActivateRpc()
    {
        _plate.transform.position = _downPoint.position;
        foreach (var connection in _connection)
        {
            connection.Activate();
        }
    }
    void Desactivate()
    {
        if (!IsOwner) return;
        DesactivateRpc();
    }
    [Rpc(SendTo.Everyone)]
    void DesactivateRpc()
    {
        _plate.transform.position = _topPoint.position;
        foreach (var connection in _connection)
        {
            connection.Activate();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<CatCharMultiplayer>();
        if (player != null)
        {
            _plate.transform.position = _downPoint.position;
            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<CatCharMultiplayer>();
        if (player != null)
        {
            _plate.transform.position = _topPoint.position;
            Desactivate();
        }
    }
}
