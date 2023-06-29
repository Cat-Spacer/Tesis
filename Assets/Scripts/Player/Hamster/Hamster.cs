using System.Collections;
using UnityEngine;
using System;

public class Hamster : MonoBehaviour
{
    private HamsterInput _controller;
    private Action _HamsterAction = delegate { };
    [SerializeField] CustomMovement _player;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private float _speed, _maxSpeed, _pointRadius, _checkRadius = 5.0f, _interactRadius = 2.5f;
    [SerializeField] private LayerMask _tubeLayerMask, _generatorLayerMask;
    [SerializeField] private bool _inTube, _gizmos = false;
    [SerializeField] private Tube _currentTube, _lastTube;
    [SerializeField] private Vector3 _currentTubePos;
    [SerializeField] private Generator[] _generators;
    [SerializeField] private Generator _generator;
    [SerializeField] private int _energyCollected, _maxEnergy = 3;
    public bool visible = true;
    [SerializeField] private GameObject _returnBTN = null;
    [SerializeField] private KeyCode _retBTNKey = KeyCode.F12;
    private bool _owlCatched;

    private void Start()
    {
        MoveWithPlayer();
        _HamsterAction = () => MoveWithPlayer(_speed * 4f);
        _generators = FindObjectsOfType<Generator>();
    }


    private void Update()
    {
        _HamsterAction();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(_retBTNKey) && !_owlCatched)
            ReturnToPlayer();
        /*if (_inTube && _returnBTN)
            if (!_returnBTN.activeInHierarchy) _returnBTN.SetActive(true);*/
    }

    public void MoveWithPlayer() { transform.position = _playerPos.position; }

    public void MoveWithPlayer(float speed) { transform.position = Vector3.MoveTowards(transform.position, _playerPos.position, speed * Time.deltaTime); }

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

    public void MoveToPosition(Vector2 pos) { transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime); }

    public void GoToPosition(Vector2 pos) { _HamsterAction = () => MoveToPosition(pos); }

    public void GetInTube(Vector2 targetPosition, Tube tube = null)
    {
        if (_inTube || _owlCatched) return;
        _player.HamsterCheck(false);

        var tubeColl = Physics2D.Raycast(targetPosition, Vector3.forward, _pointRadius, _tubeLayerMask);

        //Debug.Log($"GetInTube, tubeColl = {tubeColl.collider.gameObject.name}, targetPosition = {(Vector3)targetPosition}");
        if (tubeColl || tube)
        {
            /*
            var playerOrigPos = FindObjectOfType<CustomMovement>().gameObject.transform.position;
            var distance = 0.0f;
            if (tube)
                distance = Vector2.Distance(tube.transform.position, playerOrigPos);
            else
                distance = Vector2.Distance(tubeColl.transform.position, playerOrigPos);*/

            //Debug.Log($"distance = {distance}");
            //if (distance <= _interactRadius)
            //{
            if (!tube.IsEntry()) return;
            _HamsterAction = MoveInTubes;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            _inTube = true;
            if (_returnBTN) _returnBTN.SetActive(true);
            //}
        }
    }

    void CheckNextTube()
    {
        if (_currentTube.IsCheckpoint() || _currentTube.IsEntry() || _currentTube.IsExit())
        {
            _currentTube.GetPossiblePaths(this);
            _HamsterAction = delegate { };

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
            //if (Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask))
            //{
            //    var generator = Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)
            //    .gameObject.GetComponent<Generator>();
            //    if (generator.buttons.activeInHierarchy) _generator.buttons.SetActive(false);
            //}            

            var nextTube = _currentTube.GetNextPath(_lastTube);
            _lastTube = _currentTube;
            _currentTube = nextTube;
            _currentTubePos = _currentTube.GetCenter();
        }
    }

    public void MoveToNextTube(Tube tube)
    {
        if (tube == null) //Si no hay siguiente tubo sale del tubo
        {
            if (_generator)
            {
                Debug.Log("Prendo generador");
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
            _HamsterAction = () => MoveWithPlayer(_speed * 4f);

        if (_currentTube) _currentTube.ArrowsActDes(false);
        if (_generator) _generator.StopMiniGame();
        _generator = null;
        if (_returnBTN) _returnBTN.SetActive(false);
    }

    public bool InTube() { return _inTube; }

    public int Energy { get { return _energyCollected; } }

    public int MaxEnergy { get { return _maxEnergy; } }

    public Generator Generator { get { return _generator; } }

    public Tube LastTube { get { return _lastTube; } }
    public Tube CurrentTube { get { return _currentTube; } }

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
    private void OnDrawGizmos()
    {
        if (_gizmos) return;
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireCube(transform.position, new Vector3(_checkRadius, _checkRadius));
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }

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
}