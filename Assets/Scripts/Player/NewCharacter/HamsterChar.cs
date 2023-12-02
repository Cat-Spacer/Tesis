using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterChar : PlayerCharacter
{
    HamsterCharacterInput _inputs;
    public override void Start()
    {
        base.Start();
        _inputs = GetComponent<HamsterCharacterInput>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _TubesMovementAction();
    }
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
    private bool _inTube;
    private float _speed = -5;
    private Vector2 _currentTubePos;
    Action _TubesMovementAction = delegate { };
    Vector2 _tubeEntry;
    Tube _currentTube, _lastTube;

    public void GetInTube(Vector2 targetPosition)
    {
        if (_inTube) return;
        _inputs.ChangeToTubesInputs(true);
        _tubeEntry = targetPosition;
        _TubesMovementAction = EnterTube;
    }
    void EnterTube()
    {
        MoveToPosition(_currentTubePos);
        if (Vector3.Distance(transform.position, _currentTubePos) < .01f) CheckNextTube();
    }

    public void MoveInTubes()
    {
        MoveToPosition(_currentTubePos);
        if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
            CheckNextTube();
    }

    public void MoveToPosition(Vector2 pos)
    { transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime); }

    public void GoToPosition(Vector2 pos) { _TubesMovementAction = () => MoveToPosition(pos); }

    public void TubeDirection(Vector2 dir)
    {
        if(dir == new Vector2(1, 0))
        {
            _currentTube.GoRight();
        }
        if (dir == new Vector2(-1, 0))
        {
            _currentTube.GoLeft();
        }
        if (dir == new Vector2(0, 1))
        {
            _currentTube.GoUp();
        }
        if (dir == new Vector2(0, -1))
        {
            _currentTube.GoDown();
        }
    }

    void CheckNextTube()
    {
        if (_currentTube.IsCheckpoint() || _currentTube.IsEntry() || _currentTube.IsExit())
        {
            //_canvas.SetActive(true);
            //if (_arrows) _arrows.SetTubes();
            _currentTube.GetPossiblePaths(this);
            _TubesMovementAction = delegate { };
        }
        else
        {
            var nextTube = _currentTube.GetNextPath(_lastTube);
            _lastTube = _currentTube;
            _currentTube = nextTube;
            _currentTubePos = _currentTube.GetCenter();
        }
    }

    public void MoveToNextTube(Tube tube)
    {
        if (tube != null) //Se mueve al siguiente tubo
        {
            _TubesMovementAction = MoveInTubes;
            _lastTube = _currentTube;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            _inTube = true;
        } 
        else return; //Si no hay siguiente tubo sale del tubo
    }

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
