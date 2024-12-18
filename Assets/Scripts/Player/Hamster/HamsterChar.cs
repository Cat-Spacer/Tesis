using System;
using System.Collections;
using UnityEngine;

public class HamsterChar : PlayerCharacter
{
    [SerializeField] private Transform headTransform;
    [SerializeField] private Vector2 headSize;
    private BoxCollider2D _coll;
    public HamsterCanvas canvas;
    private bool _ifShrink = false;
    private float _jumpDefault;
    private Vector2 _defaultGroundCheckArea;

    private void Awake()
    {
        if (GameManager.Instance) GameManager.Instance.SetHamsterChar = this;
    }
    public override void Start()
    {
        base.Start();
        _coll = GetComponent<BoxCollider2D>();
        _jumpDefault = _data.jumpForce;
        _defaultGroundCheckArea = _data.groundCheckArea;
    }
    protected override void OnUpdatePoints(object[] obj)
    {
        base.OnUpdatePoints(obj);
        var type = (CharacterType)obj[0];

        if (type == CharacterType.Hamster)
        {
            _model.PlayParticle(ParticleType.ExtraScore);
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _TubesMovementAction();
    }
    #region TUBES
    private bool isMoving = false;
    private bool _inTube;
    private float _speed = 8f;
    private Vector2 _currentTubePos;
    private Action _TubesMovementAction = delegate { };
    private Vector3 _tubeEntry;
    private Tube _currentTube, _lastTube;
    
    public bool IsInTube => _inTube;

    public void GetInTube(Vector3 targetPosition, Tube tube)
    {
        if (!_ifShrink)
        {
            Special();
        }
        if (_inTube || !_ifShrink || isMoving) return;
        tube.OnPlayerEnter(true);

        _model.PlayParticle(ParticleType.Tube);
        _inTube = true;
        _coll.enabled = false;
        _rb.simulated = false;
        _tubeEntry = targetPosition;
        _currentTube = tube;
        GoToPosition(_tubeEntry);
        _TubesMovementAction += EnterTube;
        _model.PlayParticle(ParticleType.InTube);
    }

    public void GetOutOfTube(Vector2 targetPosition, Tube tube)
    {
        tube.OnPlayerEnter(false);
        _tubeEntry = targetPosition;
        _model.PlayParticle(ParticleType.Tube);
        GoToPosition(_tubeEntry);
        _TubesMovementAction += GetInWorld;
        _model.StopParticle(ParticleType.InTube);
    }

    void GetInWorld()
    {
        Debug.Log("TryGetInWorld");
        if (Vector2.Distance(transform.position, _tubeEntry) < .1f)
        {
            isMoving = false;
            _coll.enabled = true;
            _rb.simulated = true;
            _inTube = false;
            canvas.HideArrows();
            _TubesMovementAction = delegate { };
            _rb.velocity = Vector2.zero;
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
        if (!_currentTube.IsCheckpoint() || isMoving) return;
        if (dir == new Vector2(1, 0))
        {
            _model.FaceDirection(1);
            if (_currentTube.MoveRight() != null && _currentTube.MoveRight().IsEntry())
            {
                Debug.Log("Exit with Arrows");
                GetOutOfTube(_currentTube.MoveRight().transform.position, _currentTube.MoveRight());
                return;
            }
            MoveToNextTube(_currentTube.MoveRight());
        }
        if (dir == new Vector2(-1, 0))
        {
            _model.FaceDirection(-1);
            if (_currentTube.MoveLeft() != null && _currentTube.MoveLeft().IsEntry())
            {
                Debug.Log("Exit with Arrows");
                GetOutOfTube(_currentTube.MoveLeft().transform.position, _currentTube.MoveLeft());
                return;
            }
            MoveToNextTube(_currentTube.MoveLeft());
        }
        if (dir == new Vector2(0, 1))
        {
            if (_currentTube.MoveUp()!= null && _currentTube.MoveUp().IsEntry())
            {
                Debug.Log("Exit with Arrows");
                GetOutOfTube(_currentTube.MoveUp().transform.position, _currentTube.MoveUp());
                return;
            }
            MoveToNextTube(_currentTube.MoveUp());
        }
        if (dir == new Vector2(0, -1))
        {
            if (_currentTube.MoveDown() != null && _currentTube.MoveDown().IsEntry())
            {
                Debug.Log("Exit with Arrows");
                GetOutOfTube(_currentTube.MoveDown().transform.position, _currentTube.MoveDown());
                return;
            }
            MoveToNextTube(_currentTube.MoveDown());
        }
    }

    void CheckNextTube()
    {
        if (_currentTube.IsCheckpoint())
        {
            isMoving = false;
            canvas.gameObject.SetActive(true);
            canvas.CheckTubeDirections(_currentTube);
            //_currentTube.GetPossiblePaths(this);
            _TubesMovementAction = delegate { };
            SoundManager.instance.Pause(SoundsTypes.HamsterOnTubes, gameObject);
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

    private void MoveToNextTube(Tube tube)
    {
        if (!tube) return; //Se mueve al siguiente tubo
        isMoving = true;
        canvas.HideArrows();
        _lastTube = _currentTube;
        _currentTube = tube;
        _currentTubePos = tube.GetCenter();
        GoToPosition(_currentTubePos);
        _TubesMovementAction += MoveInTubes;
        SoundManager.instance.Play(SoundsTypes.HamsterOnTubes, gameObject, true);
        _inTube = true;
    }
    public bool InTube()
    {
        return _inTube;
    }
    #endregion
    bool canShrink = true;
    public override void Special()
    {
        if (!canShrink) return;
        if (!_ifShrink)
        {
            canShrink = false;
            SoundManager.instance.Play(SoundsTypes.HamsterJump, gameObject);
            _model.PlayParticle(ParticleType.Shrink);
            _ifShrink = true;
            transform.localScale = new Vector3(.5f, .5f, 1);
            _data._inventoryPos.localScale = new Vector3(1.5f, 1.5f, 1);
            _data.groundCheckArea = new Vector2(0.24f, 0.08f);
            _data.jumpForce = _jumpDefault * .75f;
            StartCoroutine(ShrinkCooldown());
        }
        else
        {
            var hit = Physics2D.OverlapBox(headTransform.position, headSize, 0, _data.groundLayer);
            if (hit != null)
            {
                _model.PlayParticle(ParticleType.Error);
                SoundManager.instance.Play(SoundsTypes.Error, gameObject);
                return; }
            canShrink = false;
            SoundManager.instance.Play(SoundsTypes.HamsterJump, gameObject);
            _model.PlayParticle(ParticleType.Grow);
            _ifShrink = false;
            transform.localScale = new Vector3(1f, 1f, 1);
            _data._inventoryPos.localScale = new Vector3(1f, 1f, 1);
            transform.position += new Vector3(0, .5f, 0);
            _data.groundCheckArea = _defaultGroundCheckArea;
            _data.jumpForce = _jumpDefault;
            StartCoroutine(ShrinkCooldown());
        }
    }

    IEnumerator ShrinkCooldown()
    {
        yield return new WaitForSeconds(.5f);
        canShrink = true;
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
