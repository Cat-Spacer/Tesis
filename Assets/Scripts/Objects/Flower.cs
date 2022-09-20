using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] float _forceUp = 60;
    [SerializeField] float _forceDown = 40;
    [SerializeField] float _strongerForceUp = 100;
    //  [SerializeField] GameObject ventiObj;
    //private float returnSpeed;
    [SerializeField] private float _defaultDrag;
    [SerializeField] private float _ventdrag = 0.3f;  //aumentar para lentitud de movimiento
    CustomMovement _player;
    Rigidbody2D _playerRb;
    [SerializeField] private Transform _flower;
    [SerializeField] private float _maxUp;
    [SerializeField] private float _maxDown;
    private float _stop = 1.7f;
   // bool _goUp;
    bool _goingDown;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<CustomMovement>();
   
        if (player == null) return;


        _player = player;

        _playerRb = _player.GetComponent<Rigidbody2D>();

        if (_playerRb == null)
            Debug.LogError("Player Rigidbody2D not found");

        SoundManager.instance.Play(SoundManager.Types.WindForest);

       _player.ForceDashEnd();
       _defaultDrag = _playerRb.drag;
        Debug.Log("drag" + _defaultDrag);
       _playerRb.drag = _ventdrag;

     //   _goUp = true;

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_player == null) return;


        var playerLocalVelY = transform.InverseTransformDirection(_playerRb.velocity).y;

        if (playerLocalVelY < 0 && _player.transform.position.y - _flower.transform.position.y <= _stop)
        {
            Debug.Log("frenar");
            _playerRb.AddForce(_player.transform.up * _strongerForceUp);
            
        }

        if (_player.transform.position.y - _flower.transform.position.y >= _maxUp)
        {
            _playerRb.AddForce(_player.transform.up *_forceDown);
            _goingDown = true;
        }    
        else if (_player.transform.position.y - _flower.transform.position.y <= _maxUp && !_goingDown)
        {
            _playerRb.AddForce(_player.transform.up * _forceUp);
        }

        if (_player.transform.position.y - _flower.transform.position.y <= _maxDown)
        {
            _goingDown = false;
        }
            
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //  _goUp = false;
        Debug.Log("drag" + _defaultDrag);
        if (_playerRb == null) return;
        _playerRb.drag = _defaultDrag;

        _playerRb = null;
        _player = null;

        SoundManager.instance.Pause(SoundManager.Types.WindForest);

    }

    /*IEnumerator RestartMove()
    {
        yield return new WaitForSeconds(0.07f);
        _playerRb.constraints = ~RigidbodyConstraints2D.FreezePosition;
        _firstIn = false;
    }*/

}
