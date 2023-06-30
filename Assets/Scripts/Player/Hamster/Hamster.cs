using System.Collections;
using UnityEngine;
using System;

public class Hamster : MonoBehaviour
{
    private HamsterInput _controller;
    private Action _HamsterAction = delegate { };
    [SerializeField] private CustomMovement _player;
    [SerializeField] private Transform _playerPos;
    [SerializeField]
    private float _speed = 3.0f, _maxSpeed = 5.0f, _aceleration = 0.0f, _pointRadius = 0f,
                                    _checkRadius = 5.0f, _interactRadius = 2.5f, _maxDistance = 2.0f;
    [SerializeField] private LayerMask _tubeLayerMask, _generatorLayerMask;
    [SerializeField] private bool _inTube, _gizmos = false;
    [SerializeField] private Tube _currentTube, _lastTube;
    [SerializeField] private Vector3 _currentTubePos;
    [SerializeField] private Generator[] _generators;
    [SerializeField] private Generator _generator;
    [SerializeField] private int _energyCollected, _maxEnergy = 3;
    public bool visible = true;
    [SerializeField] private GameObject _canvas = null;
    [SerializeField] private KeyCode _retBTNKey = KeyCode.F12;
    private bool _owlCatched;
    /*
    private HingeJoint2D _joint2D;
    private DistanceJoint2D _distanceJoint2D;*/

    private void Start()
    {
        _aceleration = 0.0f;
        if (_maxSpeed <= 0 || _maxSpeed < _speed) _maxSpeed = 2.0f * _speed;
        MoveWithPlayer();
        _HamsterAction = () => MoveWithPlayerSmoth();
        _generators = FindObjectsOfType<Generator>();
        if (!_player) _player = FindObjectOfType<CustomMovement>();
        _canvas.SetActive(false);
        /*_joint2D = GetComponent<HingeJoint2D>();
        if (_joint2D && FindObjectOfType<CustomMovement>()) _joint2D.connectedBody = FindObjectOfType<CustomMovement>().rb;
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        if (_distanceJoint2D && FindObjectOfType<CustomMovement>()) _distanceJoint2D.connectedBody = FindObjectOfType<CustomMovement>().rb;*/
    }


    private void Update()
    {
        _HamsterAction();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(_retBTNKey) && !_owlCatched)
            ReturnToPlayer();
    }

    public void MoveWithPlayer() { transform.position = _playerPos.position; }

    public void MoveWithPlayerSmoth() { transform.position = Vector3.MoveTowards(transform.position, _playerPos.position, HamsterSpeed(_speed)); }

    private float HamsterSpeed(float speed)
    {
        float distance = Vector2.Distance(transform.position, _playerPos.position);
        //Debug.Log(distance);

        /*float distanceX = _playerPos.position.x - transform.position.x; // Distance between center and new position on X axis
        float distanceZ = _playerPos.position.z - transform.position.z; // Distance between center and new position on Y axis*/

        if (distance > _maxDistance)
        {
            /*Vector2 pos = (Vector2)_playerPos.position + Vector2.one * _maxDistance;
            float angle = Mathf.Atan2(-distanceZ, -distanceX);
            float x = _playerPos.position.x + _maxDistance * Mathf.Cos(angle);
            float z = _playerPos.position.z + _maxDistance * Mathf.Sin(angle);
            Vector3 distPos = new Vector3(x, transform.position.y, z);
            //transform.position = pos;
            transform.position = Vector3.MoveTowards(transform.position, _playerPos.position, speed * Time.deltaTime);*/
            speed *= distance;
        }
        return speed * Time.deltaTime;
    }

    public void AddEnergy(int energy_arg)
    {
        _energyCollected += energy_arg;
        if (_energyCollected < 0) _energyCollected = 0;
        if (_energyCollected > _maxEnergy) _energyCollected = _maxEnergy;
        foreach (var item in _generators)
            item.SetEnergyCounter(_energyCollected);
    }

    public void MoveInTubes()
    {
        MoveToPosition(_currentTubePos);
        if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
            CheckNextTube();
    }

    public void MoveToPosition(Vector2 pos)
    { transform.position = Vector3.MoveTowards(transform.position, pos, (_speed + _aceleration) * Time.deltaTime); }

    public void GoToPosition(Vector2 pos) { _HamsterAction = () => MoveToPosition(pos); }

    public void GetInTube(Vector2 targetPosition, Tube tube = null)
    {
        if (_inTube || _owlCatched) return;
        _player.HamsterCheck(false);

        var tubeColl = Physics2D.Raycast(targetPosition, Vector3.forward, _pointRadius, _tubeLayerMask);

        if (tubeColl || tube)
        {
            if (!tube.IsEntry()) return;
            _HamsterAction = MoveInTubes;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            _inTube = true;
            if (_canvas) _canvas.SetActive(true);
        }
    }

    void CheckNextTube()
    {
        if (_currentTube.IsCheckpoint() || _currentTube.IsEntry() || _currentTube.IsExit())
        {
            _canvas.SetActive(true);
            _currentTube.GetPossiblePaths(this);
            _HamsterAction = delegate { };
            _aceleration = 0.0f;

            if (!Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)) return;
            var generator = Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)
                .gameObject.GetComponent<Generator>();
            _generator = generator;

            if (_currentTube.IsExit() && _generator)
            {
                if (_generator.EnergyNeeded <= _energyCollected || _generator.IsAlreadyStarded)
                {
                    //_generator.TurnButtons();
                    _generator.StartGenerator();
                }
            }
        }
        else
        {
            var nextTube = _currentTube.GetNextPath(_lastTube);
            _lastTube = _currentTube;
            _currentTube = nextTube;
            _currentTubePos = _currentTube.GetCenter();
            if (_aceleration + _speed <= _maxSpeed) _aceleration++;
        }
    }

    public void MoveToNextTube(Tube tube)
    {
        if (tube == null) //Si no hay siguiente tubo sale del tubo
        {
            if (_generator)
            {
                _generator.TurnButtons();
                _generator.StartGenerator();
            }
        }
        else //Se mueve al siguiente tubo
        {
            _HamsterAction = MoveInTubes;
            _lastTube = _currentTube;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            _inTube = true;
        }
    }

    public void ReturnToPlayer(bool instant = true)
    {
        _inTube = false;
        _owlCatched = false;
        if (_player) _player.HamsterCheck(true);

        if (instant)
            _HamsterAction = MoveWithPlayer;
        else
            _HamsterAction = MoveWithPlayerSmoth;

        if (_currentTube) _currentTube.ArrowsActDes(false);
        if (_generator) _generator.StopMiniGame();
        _generator = null;
        if (_canvas) _canvas.SetActive(false);
    }

    public void Die()
    {
        _player.GetDamage();
        ResetEnergy();
        ReturnToPlayer();
    }

    public void ResetEnergy()
    {
        _energyCollected = 0;
    }

    public bool InTube() { return _inTube; }

    public int Energy { get { return _energyCollected; } }
    public int MaxEnergy { get { return _maxEnergy; } }

    public Generator Generator { get { return _generator; } }

    public Tube LastTube { get { return _lastTube; } }
    public Tube CurrentTube { get { return _currentTube; } }

    public GameObject Canvas { get { return _canvas; } }

    public void OwlCatch(float time)
    {
        _owlCatched = true;
        _HamsterAction = delegate { };
        StartCoroutine(DieByOwl(time));
    }

    IEnumerator DieByOwl(float time)
    {
        yield return new WaitForSeconds(time);
        _player.GetDamage();
    }

    private void OnDrawGizmos()
    {
        if (!_gizmos) return;
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireCube(transform.position, new Vector3(_checkRadius, _checkRadius));
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}