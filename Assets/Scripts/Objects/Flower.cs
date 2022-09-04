using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] float _forceUp = 60;
    [SerializeField] float _forceDown = 40;
    //  [SerializeField] GameObject ventiObj;
    //private float returnSpeed;
    private float _defaultDrag;
    [SerializeField] private float _ventdrag = 0.3f;  //aumentar para lentitud de movimiento
    CustomMovement _player;
    Rigidbody2D _playerRb;
    [SerializeField] private Transform _flower;
    [SerializeField] private float _maxUp;
    [SerializeField] private float _maxDown;
    bool _goUp;
    bool _goingDown;


    private void Update()
    {
       // ventiObj.transform.Rotate(new Vector3(0, ventiObj.transform.position.y * rotationSpeed, 0));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hola");

        var player = other.gameObject.GetComponent<CustomMovement>();
   
        if (player == null) return;

        _player = player;

        _playerRb = _player.GetComponent<Rigidbody2D>();

        if (_playerRb == null)
            Debug.LogError("Player Rigidbody2D not found");

       _player.ForceDashEnd();
       _defaultDrag = _playerRb.drag;
       _playerRb.drag = _ventdrag;

        _goUp = true;
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_player == null) return;

        Debug.Log(_player.transform.position.y - _flower.transform.position.y);

       
        if (_player.transform.position.y - _flower.transform.position.y >= _maxUp)
        {
            _playerRb.AddForce(_player.transform.up *_forceDown);
            Debug.Log("down");
            _goingDown = true;
        }    
        else if (_player.transform.position.y - _flower.transform.position.y <= _maxUp && !_goingDown)
        {
            _playerRb.AddForce(_player.transform.up * _forceUp);
            Debug.Log("up");
        }

        if (_player.transform.position.y - _flower.transform.position.y <= _maxDown)
        {
            _goingDown = false;
        }
            
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _goUp = false;
        _playerRb.drag = _defaultDrag;
        _playerRb = null;

        _player = null;
        _playerRb = null;
    }

}
