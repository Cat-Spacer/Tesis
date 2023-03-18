using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObstacles : MonoBehaviour
{
    protected CustomMovement _player;
    protected Rigidbody2D _playerRb;
    protected Transform _playerTransform;
    protected Collider2D _playerCollider;
    protected BoxCollider2D _windCollider;
    [SerializeField] protected LayerMask _layerMask;
    protected Vector2 _windColliderStartSize;
    [SerializeField] protected float _defaultDrag;
    protected bool _blockWind = false;

    protected virtual void Start()
    {
        _player = FindObjectOfType<CustomMovement>();
        _playerTransform = _player.GetComponent<Transform>();
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _playerCollider = _player.GetComponent<Collider2D>();
        _windCollider = GetComponent<BoxCollider2D>();
        _windColliderStartSize = _windCollider.size;
        _defaultDrag = _playerRb.drag;
    }

    protected virtual void Wind(Vector2 direction_arg, float force_arg)
    {
        if (_blockWind) return;
        _playerRb.AddForce(direction_arg * force_arg);
        
    }

    protected virtual void BlockWind(Transform blocker_arg)
    {

        // if ((_layerMask & (1 << blocker_arg.gameObject.layer)) > 0)
        if (blocker_arg.gameObject.layer ==15 )
        {
            Debug.Log("BLOCK WIND");
            //_windCollider.gameObject.SetActive(false);
          //  _blockWind = true;
            OnExitFlower();
        }

        /*if (InSightSides(transform.right))
        {
            Debug.Log("BLOCK WIND right");
            _windCollider.size = new Vector2(_windCollider.size.x, transform.position.x - blocker_arg.position.x);
        }           

        if (InSightSides(transform.up))
        {
            Debug.Log("BLOCK WIND up");
            _windCollider.size = new Vector2(_windCollider.size.x, transform.position.y - blocker_arg.position.y);
        }*/


        //if detection from uppers
    }

    protected virtual void UnblockWind(Transform blocker_arg)
    {
        // if ((_layerMask & (1 << blocker_arg.gameObject.layer)) > 0)
        if (blocker_arg.gameObject.layer == 15)
        {
            Debug.Log("unblock wind");
            //  _windCollider.gameObject.SetActive(true);
            //_blockWind = false;
            OnEnterFlower();
        }
            
        //_windCollider.size = _windColliderStartSize;
    }

    protected bool InSightSides(Vector2 direction)
    {
        if (Physics2D.Raycast(transform.position, direction, _windCollider.size.y, _layerMask)) return true;
        else return false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Block Wind");
        BlockWind(other.transform);
        if (_blockWind) return;       
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        UnblockWind(other.transform);
        if (_blockWind) return;        
    }
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (_blockWind) return;
       // UnblockWind(other.transform);
    }

    protected virtual void OnExitFlower(){}

    protected virtual void OnEnterFlower(){}
}
