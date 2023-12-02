using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterChar : PlayerCharacter
{
    private bool _inTube;
    public override void Punch()
    {
        var obj = Physics2D.OverlapCircle(_data.attackPoint.position, _data.attackRange.x, _data.attackableLayer);

        if (obj == null) return;
        var attackable = obj.GetComponent<IPlayerInteract>();
        if (attackable == null) return;
        
        Debug.Log("PUNCH " + attackable);
        attackable.GetKnockback(_data.punchForce, transform.right + transform.up, _data.stunForce);
        if (LiveCamera.instance.IsOnAir())
        {
            LiveCamera.instance.ChangePeace(-1);
        }
    }

    #region TUBES

    private Vector2 _currentTubePos;
    
    public void GetInTube(Vector2 targetPosition, Tube tube = null)
    {
        if (_inTube) return;


    }
    
    // public void MoveInTubes()
    // {
    //     MoveToPosition(_currentTubePos);
    //     if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
    //         CheckNextTube();
    // }

    public void MoveToPosition(Vector2 pos)
    { transform.position = Vector3.MoveTowards(transform.position, pos, (_speed + _aceleration) * Time.deltaTime); }

    //public void GoToPosition(Vector2 pos) { _HamsterAction = () => MoveToPosition(pos); }



    // void CheckNextTube()
    // {
    //     if (_currentTube.IsCheckpoint() || _currentTube.IsEntry() || _currentTube.IsExit())
    //     {
    //         //_canvas.SetActive(true);
    //         if(_arrows) _arrows.SetTubes();
    //         _currentTube.GetPossiblePaths(this);
    //         _HamsterAction = delegate { };
    //         _aceleration = 0.0f;
    //
    //         if (!Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)) return;
    //         var generator = Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)
    //             .gameObject.GetComponent<Generator>();
    //         _generator = generator;
    //
    //         if (_currentTube.IsExit() && _generator)
    //         {
    //             if (_generator.EnergyNeeded <= _energyCollected || _generator.IsAlreadyStarded)
    //             {
    //                 //_generator.TurnButtons();
    //                 _generator.StartGenerator();
    //             }
    //         }
    //     }
    //     else
    //     {
    //         var nextTube = _currentTube.GetNextPath(_lastTube);
    //         _lastTube = _currentTube;
    //         _currentTube = nextTube;
    //         _currentTubePos = _currentTube.GetCenter();
    //         if (_aceleration + _speed <= _maxSpeed) _aceleration++;
    //     }
    // }
    //
    // public void MoveToNextTube(Tube tube)
    // {
    //     if (tube == null) //Si no hay siguiente tubo sale del tubo
    //     {
    //         if (_generator)
    //         {
    //             _generator.TurnButtons();
    //             _generator.StartGenerator();
    //         }
    //     }
    //     else //Se mueve al siguiente tubo
    //     {
    //         _HamsterAction = MoveInTubes;
    //         _lastTube = _currentTube;
    //         _currentTube = tube;
    //         _currentTubePos = tube.GetCenter();
    //         _inTube = true;
    //     }
    // }

    #endregion
    public bool InTube()
    {
        return _inTube;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_data.attackPoint.position, _data.attackRange.x);
        Gizmos.DrawWireCube(_data.groundPos.position, _data.groundCheckArea);
        Gizmos.DrawWireCube(transform.position, _data.jumpInpulseArea);
        Gizmos.DrawWireCube(transform.position, _data.interactSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionRight.position, _data.bounceSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionLeft.position, _data.bounceSize);
        
    }
}
