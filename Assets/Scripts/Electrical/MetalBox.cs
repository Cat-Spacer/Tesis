using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MetalBox : MonoBehaviour
{
    //[SerializeField] private LayerMask _collMask;
    [SerializeField] private Magnet _magnet;
    private Rigidbody2D _myRB2D = null;
    void Start()
    {
        _myRB2D = GetComponent<Rigidbody2D>();
        /*if (_magnet != null)
            _collMask = _magnet.gameObject.layer;*/
    }

    private void Update()
    {
        if (_magnet == null) return;
        if (!_magnet.active)
            _myRB2D.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_myRB2D == null || collision.gameObject.GetComponent<Magnet>() == null) return;
        _magnet = collision.gameObject.GetComponent<Magnet>();
        //_collMask = _magnet.gameObject.layer;
        _myRB2D.simulated = false;
    }
}