using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestFlower : WindObstacles
{
    [SerializeField] float _forceUp = 60;
    [SerializeField] float _forceDown = 40;
    [SerializeField] float _strongerForceUp = 100;
    [SerializeField] private float _ventdrag = 0.3f;  //aumentar para lentitud de movimiento
 
    
    [SerializeField] private Transform _flower;
    [SerializeField] private float _maxUp;
    [SerializeField] private float _maxDown;
    private float _stop = 1.7f;
    public static bool onFlower = false;
    bool _goingDown;


     protected override void OnTriggerEnter2D(Collider2D other)
     {
        base.OnTriggerEnter2D(other);

        if (other != _playerCollider) return;
        onFlower = true;
        if (_blockWind) return;

        OnEnterFlower();
    
     }


    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other != _playerCollider) return;

        _playerRb.drag = _ventdrag;
        var playerLocalVelY = transform.InverseTransformDirection(_playerRb.velocity).y;

        if (playerLocalVelY < 0 && _playerTransform.position.y - _flower.transform.position.y <= _stop)
        {
   
            Wind(_player.transform.up, _strongerForceUp);
            
        }

        if (_playerTransform.position.y - _flower.transform.position.y >= _maxUp)
        {
   
            Wind(_player.transform.up, _forceDown);
        
            _goingDown = true;
        }    
        else if (_playerTransform.position.y - _flower.transform.position.y <= _maxUp && !_goingDown)
        {
            Wind(_player.transform.up, _forceUp);

        }

        if (_playerTransform.position.y - _flower.transform.position.y <= _maxDown)
        {
            _goingDown = false;
        }
            
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        
        if (other != _playerCollider) return;

        OnExitFlower();
        onFlower = false;
    }

    protected override void OnEnterFlower()
    {
        base.OnEnterFlower();

        SoundManager.instance.Play(SoundManager.Types.WindForest);

        Debug.Log("on enter");

           
        _playerRb.drag = _ventdrag;
         _player.ForceDashEnd();
        
       

      
    }

    protected override void OnExitFlower()
    {
        base.OnExitFlower();

        
        _playerRb.drag = _defaultDrag;

        SoundManager.instance.Pause(SoundManager.Types.WindForest);
   
    }



}
