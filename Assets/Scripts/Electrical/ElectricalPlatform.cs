using UnityEngine;
using System;

public class ElectricalPlatform : MonoBehaviour, IElectric, IMouseOver
{
    #region Varaibles
    //[SerializeField] private Transform _PointATop = default, _PointBLow = default;
    [SerializeField] private Transform[] _checkpoints = default;
    [SerializeField] private int _current = default, _max = default;
    private Transform _startPos = default;
    private int _startCheckpoint = default;

    //private Vector3 _newPos = default;
    [SerializeField] private float _speed = default/*, _moveScale = 3.0f*/;
    [SerializeField] private GameObject _canvas = default, _connectionSource = default;

    [SerializeField] private Vector2 _checkBottom = default;
    [SerializeField] private Vector3 _offsetBottom = default;
    [SerializeField] private LayerMask _playerLayerMask = default;

    [SerializeField] private bool _turnOn = false;

    private Action _MoveAction = delegate { };
    private LineRenderer _myLineConnection = default;
    private IGenerator _myGen = default;
    [SerializeField] private Sprite _onSprite = default, _offSprite = default;
    [SerializeField] private SpriteRenderer _sp = default;
    #endregion

    void Awake()
    {
        //_newPos = transform.position;
        if (_canvas)
        {
            if (_turnOn) { _canvas.gameObject.SetActive(true); }
            else { _canvas.gameObject.SetActive(false); }
        }

    }
    private void Start()
    {
        if (EventManager.Instance)
            EventManager.Instance.Subscribe("PlayerDeath", ResetPlatform);
        _max = _checkpoints.Length - 1;
        _startCheckpoint = _current;
        _startPos = transform;
    }
    public void TurnOff()
    {
        _turnOn = false;
        _canvas.gameObject.SetActive(false);
        _sp.sprite = _offSprite;
    }

    public Transform ConnectionSource()
    {
        return _connectionSource.transform;
    }
    public void SetGenerator(IGenerator gen, LineRenderer line)
    {
        _myGen = gen;
        _myLineConnection = line;
    }
    public void TurnOn()
    {
        _turnOn = true;
        _canvas.gameObject.SetActive(true);
        _sp.sprite = _onSprite;
    }

    public void NewClickPosition(Transform pos_arg) { }

    void Update()
    {
        //if (_turnOn && transform.position.y <= _PointATop.position.y && transform.position.y >= _PointBLow.position.y)
        //{
        //    if (_newPos.y <= _PointATop.position.y && _newPos.y >= _PointBLow.position.y)
        //    {
        //        Vector3 newYPos = new Vector3(transform.position.x, _newPos.y, transform.position.z);

        //        transform.position = Vector3.MoveTowards(transform.position, newYPos, _speed * Time.deltaTime);
        //    }
        //}
        _MoveAction();
    }

    public void UpArrow()
    {
        //_newPos.y += _moveScale;
        //if (_newPos.y > _PointATop.position.y)
        //    _newPos.y = _PointATop.position.y;
        if (_current < _max)
        {
            _current++;
            _MoveAction = GoUp;
            _canvas.gameObject.SetActive(false);
        }
    }

    void GoUp()
    {
        //var dist = Vector2.Distance(transform.position, _PointATop.position);
        //if (dist > 0.05)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _PointATop.position, _speed * Time.deltaTime);
        //}
        //else
        //{
        //    if (_turnOn)
        //    {
        //        _canvas.gameObject.SetActive(true);
        //    }
        //    _MoveAction = delegate { };
        //}

        var dist = Vector2.Distance(transform.position, _checkpoints[_current].position);
        if (dist > 0.05)
        {
            transform.position = Vector3.MoveTowards(transform.position, _checkpoints[_current].position, _speed * Time.deltaTime);
        }
        else
        {
            if (_turnOn)
            {
                _canvas.gameObject.SetActive(true);
            }
            _MoveAction = delegate { };
        }
    }

    public void DownArrow()
    {
        //_newPos.y -= _moveScale;
        //if (_newPos.y < _PointBLow.position.y)
        //    _newPos.y = _PointBLow.position.y;
        if (_current > 0)
        {
            _current--;
            _MoveAction = GoDown;
            _canvas.gameObject.SetActive(false);
        }
    }

    void GoDown()
    {
        //if (Physics2D.OverlapBox(transform.position + _offsetBottom, _checkBottom, 0, _playerLayerMask)) return;
        //var dist = Vector2.Distance(transform.position, _PointBLow.position);
        //if (dist > 0.05)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _PointBLow.position, _speed * Time.deltaTime);
        //}
        //else
        //{
        //    if (_turnOn)
        //    {
        //        _canvas.gameObject.SetActive(true);
        //    }
        //    _MoveAction = delegate { };
        //}
        if (Physics2D.OverlapBox(transform.position + _offsetBottom, _checkBottom, 0, _playerLayerMask)) return;
        var dist = Vector2.Distance(transform.position, _checkpoints[_current].position);
        if (dist > 0.05)
        {
            transform.position = Vector3.MoveTowards(transform.position, _checkpoints[_current].position, _speed * Time.deltaTime);
        }
        else
        {
            if (_turnOn)
            {
                _canvas.gameObject.SetActive(true);
            }
            _MoveAction = delegate { };
        }
    }

    public void ResetPlatform(params object[] parameters)
    {
        TurnOff();
        transform.position = _startPos.position;
        _current = _startCheckpoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _offsetBottom, _checkBottom);
    }

    public void MouseOver()
    {
        _myGen.ShowLineConnection(_myLineConnection);
    }

    public void MouseExit()
    {
        _myGen.NotShowLineConnection(_myLineConnection);
    }

    public void Interact()
    {

    }
}