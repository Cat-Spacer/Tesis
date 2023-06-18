using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    /*// player move by RB2D
    private CustomMovement _customMovement = null;
    private Rigidbody2D _playerRB = null, _platformRB = null;
    private bool _isOnPlatform = false, _hoizontalPlat = false;
    private float _orgFriction = 0.0f;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _customMovement = GetComponent<CustomMovement>();
        if (_playerRB)
            _orgFriction = _playerRB.sharedMaterial.friction = 0;
    }

    private void FixedUpdate()
    {
        if (_isOnPlatform && _playerRB && _platformRB)
        {
            if (!_customMovement.Runing)
            {
                if (_hoizontalPlat)
                    _playerRB.velocity = new Vector2(_platformRB.velocity.x, _playerRB.velocity.y);
                else
                    _playerRB.velocity = new Vector2(_playerRB.velocity.x, _platformRB.velocity.y);

                _playerRB.sharedMaterial.friction = _platformRB.sharedMaterial.friction;
            }
            else
                _playerRB.sharedMaterial.friction = _orgFriction;
        }
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        var movPlat = collision.gameObject.GetComponentInChildren<MovingPlatform>();
        var elecPlat = collision.gameObject.GetComponentInChildren<ElectricalPlatform>();
        if (!(movPlat || elecPlat)) return;
        Debug.Log("Hay colision");
        transform.SetParent(collision.transform);
        /*// player move by RB2D
        if (movPlat) _hoizontalPlat = true;
        else if (elecPlat) _hoizontalPlat = false;

        _platformRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_playerRB)
            _playerRB.sharedMaterial.friction = _platformRB.sharedMaterial.friction;
        _isOnPlatform = true;*/
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var movPlat = collision.gameObject.GetComponent<MovingPlatform>();
        var elecPlat = collision.gameObject.GetComponent<ElectricalPlatform>();
        if (!(movPlat || elecPlat)) return;

        transform.SetParent(null);
        /*// player move by RB2D
        if (movPlat) _hoizontalPlat = true;
        else if (elecPlat) _hoizontalPlat = false;

        _platformRB = null;
        //_customMovement = null;
        if (_playerRB)
            _playerRB.sharedMaterial.friction = _orgFriction;
        _isOnPlatform = false;*/
    }
}
