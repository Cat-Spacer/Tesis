using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    Hamster _hamster;
    [SerializeField] GameObject arrows;
    [SerializeField] Tube _UpTube, _RightTube, _DownTube, _LeftTube;
    [SerializeField] List<Tube> _possiblePaths = new List<Tube>();
    [SerializeField] bool _UpConnection, _RightConnection, _DownConnection, _LeftConnection;
    [SerializeField] Tube _nextTube;
    [SerializeField] Tube _lastTube;
    [SerializeField] Vector3 center;
    [SerializeField] LayerMask _tubeMask;
    [SerializeField] bool _checkpoint,_entry,_exit;
    private void Start()
    {
        center = transform.position;
        CheckNeighborTubes();       
    }
    public void GetPossiblePaths(Hamster ham)
    {
        _hamster = ham;
        arrows.SetActive(true);
    }
    public void GoUp()
    {
        _hamster.MoveToNextTube(_UpTube);
        arrows.SetActive(false);
    }
    public void GoRight()
    {
        _hamster.MoveToNextTube(_RightTube);
        arrows.SetActive(false);
    }
    public void GoDown()
    {
        _hamster.MoveToNextTube(_DownTube);
        arrows.SetActive(false);

    }
    public void GoLeft()
    {
        _hamster.MoveToNextTube(_LeftTube);
        arrows.SetActive(false);
    }
    public Tube GetNextPath(Tube lastTube)
    {
        _lastTube = lastTube;
        foreach (var tube in _possiblePaths)
        {
            if (tube != lastTube)
            {
                _nextTube = tube;
                break;
            }
        }
        if (_nextTube != null) return _nextTube;
        else return _lastTube;
    } 
    void CheckNeighborTubes()
    {
        var counter = 0;
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 1, _tubeMask);
        if (hitUp && _UpConnection)
        {
            _UpTube = hitUp.transform.gameObject.GetComponent<Tube>();
            _possiblePaths.Add(_UpTube);
            counter++;
        }
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 1, _tubeMask);
        if (hitRight && _RightConnection)
        {
            _RightTube = hitRight.transform.gameObject.GetComponent<Tube>();
            _possiblePaths.Add(_RightTube);
            counter++;
        }
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1, _tubeMask);
        if (hitDown && _DownConnection)
        {
            _DownTube = hitDown.transform.gameObject.GetComponent<Tube>();
            _possiblePaths.Add(_DownTube);
            counter++;
        }
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1, _tubeMask);
        if (hitLeft && _LeftConnection)
        {
            _LeftTube = hitLeft.transform.gameObject.GetComponent<Tube>();
            _possiblePaths.Add(_LeftTube);
            counter++;
        }

    }
    public Vector3 GetCenter()
    {
        return center;
    }
    public bool IsEntry()
    {
        return _entry;
    }
    public bool IsExit()
    {
        return _exit;
    }
    public bool IsCheckpoint()
    {
        return _checkpoint;
    }
}
