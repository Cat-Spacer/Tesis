using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HamsterCharMultiplayer : PlayerCharacterMultiplayer
{
    private BoxCollider2D _coll;
    public HamsterCanvas canvas;
    public override void Start()
    {
        base.Start();
        _coll = GetComponent<BoxCollider2D>();
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
        var player = attackable.GetNetworkObject();
        if (player == null) return;
        PunchRpc(player);
        if (LiveCameraNetwork.Instance.IsOnAir())
        {
            LiveCameraNetwork.Instance.ChangePeace(-1);
        }
    }
    [Rpc(SendTo.Everyone)]
    void PunchRpc(NetworkObjectReference player)
    {
        Debug.Log("Punch");
        player.TryGet(out NetworkObject playerNetworkObject);
        playerNetworkObject.GetComponent<IPlayerInteract>().GetKnockback(_data.punchForce, transform.right + transform.up, _data.stunForce);
        //EventManager.Instance.Trigger("OnPunchPlayer", true);
    }
    #region TUBES
    private bool _inTube;
    private float _speed = 8f;
    private Vector2 _currentTubePos;
    Action _TubesMovementAction = delegate { };
    Vector2 _tubeEntry;
    TubeSystem _currentTubeSystem, _lastTubeSystem;

    public void GetInTube(Vector2 targetPosition, TubeSystem tubeSystem)
    {
        if (_inTube) return;
        _inTube = true;
        _coll.enabled = false;
        _rb.simulated = false;
        //_inputs.ChangeToTubesInputs(true);
        _tubeEntry = targetPosition;
        _currentTubeSystem = tubeSystem;
        //GoToPosition(_tubeEntry);
        //TubesMovementAction += EnterTube;
    }

    public void GetOutOfTube(Vector2 targetPosition)
    {
        _tubeEntry = targetPosition;
        //GoToPosition(_tubeEntry);
        _TubesMovementAction += GetInWorld;
    }

    void GetInWorld()
    {
        if (Vector2.Distance(transform.position, _tubeEntry) < .1f)
        {
            _coll.enabled = true;
            _rb.simulated = true;
            _inTube = false;
            canvas.HideArrows();
            //_inputs.ChangeToTubesInputs(false);
            _TubesMovementAction = delegate {  };
        }
    }
    // void EnterTube()
    // {
    //     if (Vector2.Distance(transform.position, _tubeEntry) < .1f)
    //     {
    //         MoveToNextTube(_currentTubeSystem);
    //     }
    // }
    //
    // public void MoveInTubes()
    // {
    //     if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
    //         CheckNextTube();
    // }
    //
    // void MoveToPosition(Vector2 pos)
    // { transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime); }
    //
    // public void GoToPosition(Vector2 pos) { _TubesMovementAction = () => MoveToPosition(pos); }
    //
    // public void TubeDirection(Vector2 dir)
    // {
    //     if (!_currentTubeSystem.IsCheckpoint()) return;
    //     if(dir == new Vector2(1, 0))
    //     {
    //         MoveToNextTube(_currentTubeSystem.GoRight());
    //     }
    //     if (dir == new Vector2(-1, 0))
    //     {
    //         MoveToNextTube(_currentTubeSystem.GoLeft());
    //     }
    //     if (dir == new Vector2(0, 1))
    //     {
    //         MoveToNextTube(_currentTubeSystem.GoUp());
    //     }
    //     if (dir == new Vector2(0, -1))
    //     {
    //         MoveToNextTube(_currentTubeSystem.GoDown());
    //     }
    // }
    //
    // void CheckNextTube()
    // {
    //     if (_currentTubeSystem.IsCheckpoint() || _currentTubeSystem.IsEntry() || _currentTubeSystem.IsExit())
    //     {
    //         canvas.gameObject.SetActive(true);
    //         canvas.CheckTubeDirections(_currentTubeSystem);
    //         //_currentTube.GetPossiblePaths(this);
    //         _TubesMovementAction = delegate { };
    //     }
    //     else
    //     {
    //         // var nextTube = _currentTube.GetNextPath(_lastTube);
    //         // _lastTube = _currentTube;
    //         // _currentTube = nextTube;
    //         // _currentTubePos = _currentTube.GetCenter();
    //         MoveToNextTube(_currentTubeSystem.GetNextPath(_lastTubeSystem));
    //     }
    // }
    //
    // public void MoveToNextTube(TubeSystem tubeSystem)
    // {
    //     if (tubeSystem != null) //Se mueve al siguiente tubo
    //     {
    //         canvas.HideArrows();
    //         _lastTubeSystem = _currentTubeSystem;
    //         _currentTubeSystem = tubeSystem;
    //         _currentTubePos = tubeSystem.GetCenter();
    //         GoToPosition(_currentTubePos);
    //         _TubesMovementAction += MoveInTubes;
    //         _inTube = true;
    //     } 
    //     else return; //Si no hay siguiente tubo sale del tubo
    // }
    public bool InTube()
    {
        return _inTube;
    }
    #endregion

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
