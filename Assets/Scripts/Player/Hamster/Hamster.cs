using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static UnityEditor.PlayerSettings;

public class Hamster : MonoBehaviour
{
    private HamsterInput _controller;
    Action _HamsterAction = delegate { };
    [SerializeField] CustomMovement _player;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private float _speed, _maxSpeed, _pointRadius, _checkRadius = 5.0f, _interactRadius = 2.5f;
    [SerializeField] private LayerMask _tubeLayerMask, _generatorLayerMask;
    [SerializeField] private bool _inTube, _gizmos = false;
    [SerializeField] private Tube _currentTube, _lastTube;
    [SerializeField] private Vector3 _currentTubePos;
    [SerializeField] private Generator[] _generators;
    [SerializeField] private Generator _generator;
    [SerializeField] private int _energyCollected;
    public bool visible = true;
    bool _owlCatched;

    private void Start()
    {
        _HamsterAction = MoveWithPlayer;
        _generators = FindObjectsOfType<Generator>();
    }

    public void AddEnergy(int energy_arg) 
    {
        _energyCollected += energy_arg;
        foreach (var item in _generators)
        {
            item.SetEnergyCounter(_energyCollected);
        }

        Debug.Log(_energyCollected);
    }

    private void Update()
    {
        _HamsterAction();
    }

    private void LateUpdate()
    {

        if (Input.GetMouseButtonDown(1) && !_owlCatched)
        {
            ReturnToCat();
        }
    }

    public void MoveWithPlayer() { transform.position = _playerPos.position; }

    public void MoveInTubes()
    {
        //Vector3 newDir = Vector3.Lerp(transform.position, _currentTubePos, _speed * Time.deltaTime);
        //Vector3.ClampMagnitude(newDir, _maxSpeed);
        //transform.position = newDir;
        //transform.position = Vector3.MoveTowards(transform.position, _currentTubePos, _speed * Time.deltaTime);
        MoveToPosition(_currentTubePos);

        if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
            CheckNextTube();
    }

    public void MoveToPosition(Vector2 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime);
    }

    public void GetInTube(Vector2 targetPosition, Tube tube = null)
    {
        if (_inTube || _owlCatched) return;
        _player.HamsterCheck(false);
        //var tubeColl = Physics2D.OverlapPoint(targetPosition, _tubeLayerMask);
        //var tubeColl = Physics2D.OverlapCircle(targetPosition, _pointRadius, _tubeLayerMask);
        var tubeColl = Physics2D.Raycast(targetPosition, Vector3.forward, _pointRadius, _tubeLayerMask);

        //Debug.Log($"GetInTube, tubeColl = {tubeColl.collider.gameObject.name}, targetPosition = {(Vector3)targetPosition}");
        if (tubeColl || tube)
        {
            var playerOrigPos = FindObjectOfType<CustomMovement>().gameObject.transform.position;
            var distance = 0.0f;
            if (tube)
                distance = Vector2.Distance(tube.transform.position, playerOrigPos);
            else
                distance = Vector2.Distance(tubeColl.transform.position, playerOrigPos);

            //Debug.Log($"distance = {distance}");
            if (distance <= _interactRadius)
            {
                ///var getTube = tubeColl.collider.gameObject.GetComponent<Tube>();
                if (!tube.IsEntry()) return;
                _HamsterAction = MoveInTubes;
                _currentTube = tube;
                _currentTubePos = tube.GetCenter();
                _inTube = true;
            }
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

            if (_currentTube.IsExit() && _generator != null && _generator.EnergyNeeded <= _energyCollected)
            {
                _generator.TurnButtons();
                _generator.StartGenerator();
                // _energyCollected -= _generator.EnergyNeeded;
                //_generator.StartGenerator();
                // _generator.buttons.SetActive(true);
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
                _generator.TurnButtons();
                _generator.StartGenerator();
            }

            //Debug.Log($"tube = {tube}");
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

    public void ReturnToCat()
    {
        _inTube = false;
        _owlCatched = false;
        if (_player) _player.HamsterCheck(true);
        _HamsterAction = MoveWithPlayer;
        if (_currentTube) _currentTube.ArrowsActDes(false);
    }

    public void HamsterCatched()
    {
        _inTube = false;
        _HamsterAction = MoveWithPlayer;
        _currentTube.ArrowsActDes(false);
    }

    public bool InTube() { return _inTube; }

    public int Energy { get { return _energyCollected; } }

    public Tube LastTube { get { return _lastTube; } }

    public void Die()
    {
        _player.GetDamage();
        ResetToPlayer();
    }
    public void ResetToPlayer()
    {
        ReturnToCat();
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