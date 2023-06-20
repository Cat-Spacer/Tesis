using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CustomMovement))]
public class PlatformMovement : MonoBehaviour
{
    private CustomMovement _customMovement = null;
    private Rigidbody2D _playerRB = null, _moverRB = null;
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
        if (_isOnPlatform && _playerRB && _moverRB)
            if (!_customMovement.Runing) _playerRB.velocity = new Vector2(_moverRB.velocity.x, _playerRB.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var movOColl = collision.gameObject.GetComponentInChildren<MoveOnCollision>();
        if (!movOColl) return;

        _moverRB = collision.gameObject.GetComponent<Rigidbody2D>();
        _isOnPlatform = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var movOColl = collision.gameObject.GetComponentInChildren<MoveOnCollision>();
        if (!movOColl) return;

        _moverRB = null;
        _isOnPlatform = false;
    }

    /*public void Old()
    {

        if (_isOnPlatform && _playerRB && _platformRB)
        {
            if (!_customMovement.Runing)
            {
                if (_hoizontalPlat)
                _playerRB.velocity = new Vector2(_platformRB.velocity.x, _playerRB.velocity.y);
                else
                    _playerRB.velocity = new Vector2(_playerRB.velocity.x, _platformRB.velocity.y);

                _playerRB.sharedMaterial.friction = _platformRB.sharedMaterial.friction * 0.5f;
            }
            else
                _playerRB.sharedMaterial.friction = _orgFriction;
        }
    }*/
}
