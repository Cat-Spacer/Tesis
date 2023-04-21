using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRB, _platformRB;
    private bool _isOnPlatform;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isOnPlatform)
            _playerRB.velocity = new Vector2(_platformRB.velocity.x, _playerRB.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _platformRB = collision.gameObject.GetComponent<Rigidbody2D>();
            _isOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _platformRB = null;
            _isOnPlatform = false;
        }
    }
}
