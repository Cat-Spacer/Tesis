using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Hamster : MonoBehaviour
{
    private HamsterInput _controller;
    Action _HamsterAction = delegate { };
    [SerializeField] private Transform _playerPos;
    [SerializeField] private float _speed, _maxSpeed, _pointRadius, _checkRadius = 5.0f, _interactRadius = 2.5f;
    [SerializeField] private LayerMask _tubeLayerMask, _generatorLayerMask;
    [SerializeField] private bool _inTube;
    [SerializeField] private Tube _currentTube, _lastTube;
    [SerializeField] private Vector3 _currentTubePos;
    [SerializeField] private Generator _generator;
    [SerializeField] private int _energyCollected;
    public bool visible = true;


    private void Start()
    {
        _controller = new HamsterInput(this);
        _HamsterAction = MoveWithPlayer;
        //_testGenerator = FindObjectOfType<Generator>();
    }

    public void AddEnergy(int energy_arg)
    {
        //Debug.Log("Hamster add energy");
        _energyCollected += energy_arg;
    }

    private void Update()
    {
        _HamsterAction();
    }

    private void LateUpdate()
    {
        _controller.OnUpdate();

        if (Input.GetMouseButtonDown(1))
        {
            ReturnToCat();
        }
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

    public void GetInTube(Vector2 targetPosition, Tube tube = null)
    {
        if (_inTube) return;
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

            if ((_currentTube.IsExit() || _currentTube.IsCheckpoint()) && _generator != null && _generator.EnergyNeeded <= _energyCollected)
            {
                _energyCollected -= _generator.EnergyNeeded;
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
        //Debug.Log($"tube = {tube}");
        if (tube == null) //Si no hay siguiente tubo sale del tubo
        {
            if (_generator)
            {
                _generator.TurnButtons();
                /*foreach (var partc in _generator.electricParticles)
                    partc.Play();*/
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
        _HamsterAction = MoveWithPlayer;
    }

    public void HamsterCatched()
    {
        _inTube = false;
        _currentTube.ArrowsActDes(false);
        _HamsterAction = MoveWithPlayer;
    }

    public bool InTube() { return _inTube; }

    public int Energy { get { return _energyCollected; } }

    public Tube LastTube { get { return _lastTube;} }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireCube(transform.position, new Vector3(_checkRadius, _checkRadius));
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
