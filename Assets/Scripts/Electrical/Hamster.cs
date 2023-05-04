using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hamster : MonoBehaviour
{
    Action _HamsterAction = delegate { };
    [SerializeField] Transform _playerPos;
    [SerializeField] float _speed, _maxSpeed;
    HamsterInput _controller;
    [SerializeField] float _pointRadius, _checkRadius = 5.0f, _interactRadius = 2.5f;
    [SerializeField] LayerMask _tubeLayerMask, _generatorLayerMask;
    [SerializeField] bool _inTube;
    [SerializeField] Tube _currentTube;
    [SerializeField] Tube _lastTube;
    [SerializeField] Vector3 _currentTubePos;
    [SerializeField] Generator _testGenerator;
    [SerializeField] int _energyCollected;

    private void Start()
    {
        _controller = new HamsterInput(this);
        _HamsterAction = MoveWithPlayer;
        //_testGenerator = FindObjectOfType<Generator>();
    }

    public void AddEnergy(int energy_arg)
    {
        Debug.Log("Hamster add energy");
        _energyCollected += energy_arg;
    }
    private void Update()
    {
        _HamsterAction();
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        _controller.OnUpdate();
    }
    public void MoveWithPlayer()
    {
        transform.position = _playerPos.position;
    }
    public void MoveInTubes()
    {
        //Vector3 newDir = Vector3.Lerp(transform.position, _currentTubePos, _speed * Time.deltaTime);
        //Vector3.ClampMagnitude(newDir, _maxSpeed);
        //transform.position = newDir;
        transform.position = Vector3.MoveTowards(transform.position, _currentTubePos, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTubePos) < .01f)
        {
            CheckNextTube();
        }
    }
    public void GetInTube(Vector3 targetPosition)
    {
        if (_inTube) return;
        var tubeColl = Physics2D.OverlapCircle(targetPosition, _pointRadius, _tubeLayerMask);
        //Debug.Log($"GetInTube, tubeColl = {tubeColl}, targetPosition = {targetPosition}");
        if (tubeColl)
        {
            var playerOrigPos = FindObjectOfType<CustomMovement>().gameObject.transform.position;
            var distance = Vector2.Distance(tubeColl.transform.position, playerOrigPos);
            ///Debug.Log($"distance = {distance}");
            if (distance <= _interactRadius)
            {
                var tube = tubeColl.gameObject.GetComponent<Tube>();
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
        if (_currentTube.IsCheckpoint() || _currentTube.IsEntry())
        {
            _currentTube.GetPossiblePaths(this);
            _HamsterAction = delegate { };
            if (!Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)) return;
            var generator = Physics2D.OverlapCircle(transform.position, _checkRadius, _generatorLayerMask)
                .gameObject.GetComponent<Generator>();
            _testGenerator = generator;
            if (_currentTube.IsExit() && _testGenerator != null && _testGenerator.EnergyNeeded <= _energyCollected)
            {
                _energyCollected -= _testGenerator.EnergyNeeded;
                _testGenerator.StartGenerator();
            }
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
        //Debug.Log($"tube = {tube}");
        if (tube == null) //Si no hay siguiente tubo sale del tubo
        {
            _inTube = false;
            _HamsterAction = MoveWithPlayer;
            Debug.Log($"tube = {tube}");
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

    public void HamsterCatched()
    {
        _inTube = false;
        _currentTube.ArrowsActDes(false);
        _HamsterAction = MoveWithPlayer;
    }

    public bool InTube() { return _inTube; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireCube(transform.position, new Vector3(_checkRadius, _checkRadius));
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
