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
    [SerializeField] float _pointRadius;
    [SerializeField] LayerMask _tubeLayerMask;
    bool inTube;
    [SerializeField] Tube _currentTube;
    [SerializeField] Tube _lastTube;
    [SerializeField] Vector3 _currentTubePos;

    private void Start()
    {
        _controller = new HamsterInput(this);
        _HamsterAction = MoveWithPlayer;
    }
    private void Update()
    {
        _controller.OnUpdate();
        _HamsterAction();
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
        if (inTube) return;
        var tubeColl = Physics2D.OverlapCircle(targetPosition, _pointRadius, _tubeLayerMask);
        if (tubeColl)
        {
            var tube = tubeColl.gameObject.GetComponent<Tube>();
            if (!tube.IsEntry()) return;
            Debug.Log("InTube");
            _HamsterAction = MoveInTubes;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            inTube = true;
        }
    }
    void CheckNextTube()
    {
        if (_currentTube.IsCheckpoint() || _currentTube.IsEntry())
        {
            _currentTube.GetPossiblePaths(this);
            _HamsterAction = delegate { };
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
        if(tube == null) //Si no hay siguiente tubo sale del tubo
        {
            inTube = false;
            _HamsterAction = MoveWithPlayer;
        }
        else //Se mueve al siguiente tubo
        {
            _HamsterAction = MoveInTubes;
            _lastTube = _currentTube;
            _currentTube = tube;
            _currentTubePos = tube.GetCenter();
            inTube = true;
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(targetPosition, _pointRadius);
    }
}
