using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private CustomMovement _customMovement;
    private Rigidbody2D _playerRB, _platformRB;
    private bool _isOnPlatform;
    private float _orgFriction;

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
                _playerRB.velocity = new Vector2(_platformRB.velocity.x, _playerRB.velocity.y);
                _playerRB.sharedMaterial.friction = _platformRB.sharedMaterial.friction;
            }
            else
                _playerRB.sharedMaterial.friction = _orgFriction;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _platformRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (_playerRB)
                _playerRB.sharedMaterial.friction = _platformRB.sharedMaterial.friction;
            _isOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _platformRB = null;
            //_customMovement = null;
            if (_playerRB)
                _playerRB.sharedMaterial.friction = _orgFriction;
            _isOnPlatform = false;
        }
    }
}
