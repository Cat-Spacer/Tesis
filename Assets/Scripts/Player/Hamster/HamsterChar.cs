using System;
using UnityEngine;

public class HamsterChar : PlayerCharacter
{
    [SerializeField] private Transform headTransform;
    [SerializeField] private Vector2 headSize; 
    private BoxCollider2D _coll;
    public HamsterCanvas canvas;
    private bool _ifShrink = false;
    private float jumpDefault;
    public override void Start()
    {
        base.Start();
        _coll = GetComponent<BoxCollider2D>();
        jumpDefault = _data.jumpForce;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _TubesMovementAction();
    }
    #region TUBES
    private bool _inTube;
    private float _speed = 8f;
    private Vector2 _currentTubePos;
    Action _TubesMovementAction = delegate { };
    Vector3 _tubeEntry;
    Tube _currentTube, _lastTube;

    public void GetInTube(Vector3 targetPosition, Tube tube)
    {
        if (_inTube) return;
        _inTube = true;
        _coll.enabled = false;
        _rb.simulated = false;
        _tubeEntry = targetPosition;
        _currentTube = tube;
        GoToPosition(_tubeEntry);
        _TubesMovementAction += EnterTube;
    }

    public void GetOutOfTube(Vector2 targetPosition)
    {
        Debug.Log("GetOutOfTube");
        _tubeEntry = targetPosition;
        GoToPosition(_tubeEntry);
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
    void EnterTube()
    {
        if (Vector2.Distance(transform.position, _tubeEntry) < .1f)
        {
            MoveToNextTube(_currentTube);
        }
    }

    void MoveInTubes()
    {
        if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
            CheckNextTube();
    }

    void MoveToPosition(Vector2 pos)
    { transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime); }

    void GoToPosition(Vector2 pos) { _TubesMovementAction = () => MoveToPosition(pos); }

    public void TubeDirection(Vector2 dir)
    {
        if (!_currentTube.IsCheckpoint()) return;
        if(dir == new Vector2(1, 0))
        {
            MoveToNextTube(_currentTube.MoveRight());
        }
        if (dir == new Vector2(-1, 0))
        {
            MoveToNextTube(_currentTube.MoveLeft());
        }
        if (dir == new Vector2(0, 1))
        {
            MoveToNextTube(_currentTube.MoveUp());
        }
        if (dir == new Vector2(0, -1))
        {
            MoveToNextTube(_currentTube.MoveDown());
        }
    }
    
    void CheckNextTube()
    {
        if (_currentTube.IsCheckpoint())
        {
            canvas.gameObject.SetActive(true);
            canvas.CheckTubeDirections(_currentTube);
            //_currentTube.GetPossiblePaths(this);
            _TubesMovementAction = delegate { };
        }
        else
        {
            // var nextTube = _currentTube.GetNextPath(_lastTube);
            // _lastTube = _currentTube;
            // _currentTube = nextTube;
            // _currentTubePos = _currentTube.GetCenter();
            MoveToNextTube(_currentTube.GetNextPath(_lastTube));
        }
    }
    
    void MoveToNextTube(Tube tube)
    {
        if (tube != null) //Se mueve al siguiente tubo
        {
            canvas.HideArrows();
            _lastTube = _currentTube;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            GoToPosition(_currentTubePos);
            _TubesMovementAction += MoveInTubes;
            _inTube = true;
        } 
    }
    public bool InTube()
    {
        return _inTube;
    }
    #endregion
    public override void Special()
    {
        if (!_ifShrink)
        {
            _ifShrink = true;
            _coll.edgeRadius = .16f;
            transform.localScale = new Vector3(.5f, .5f, 1);
            _data.jumpForce = jumpDefault * .75f;
        }
        else
        {
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1.5f, 6);
            // Debug.Log(hit.collider);
            var hit = Physics2D.OverlapBox(headTransform.position, headSize, 0, _data.groundLayer);
            Debug.Log(hit);
            if (hit != null) return;
            _ifShrink = false;
            _coll.edgeRadius = .33f;
            transform.localScale = new Vector3(1f, 1f, 1);
            _data.jumpForce = jumpDefault;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_data.attackPoint.position, _data.attackRange.x);
        Gizmos.DrawWireCube(_data.groundPos.position, _data.groundCheckArea);
        Gizmos.DrawWireCube(headTransform.position, headSize);
        //Gizmos.DrawWireCube(transform.position, _data.jumpInpulseArea);
        //Gizmos.DrawWireCube(transform.position, _data.interactSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionRight.position, _data.bounceSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionLeft.position, _data.bounceSize);
        
    }
}
