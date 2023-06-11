using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tube : MonoBehaviour
{
    Hamster _hamster;
    [SerializeField] private GameObject arrows;
    [SerializeField] private Tube _UpTube, _RightTube, _DownTube, _LeftTube;
    [SerializeField] private List<Tube> _possiblePaths = new List<Tube>();
    [SerializeField] private bool _UpConnection, _RightConnection, _DownConnection, _LeftConnection;
    [SerializeField] private Tube _nextTube, _lastTube;
    [SerializeField] private Vector3 center;
    [SerializeField] private LayerMask _tubeMask;
    [SerializeField] private float _searchRad = 0.25f;

    [SerializeField] private bool _checkpoint, _entry, _exit, _gizmos = true;

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

    public void CheckNeighborTubes()
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

    public void ArrowsActDes(bool set = false) { arrows.SetActive(set); }

    public Vector3 GetCenter() { return center; }

    public bool IsEntry() { return _entry; }

    public bool IsExit() { return _exit; }

    public bool IsCheckpoint() { return _checkpoint; }

    public Tube GetUp() { return _UpTube; }
    public Tube GetDown() { return _DownTube; }
    public Tube GetLeft() { return _LeftTube; }
    public Tube GetRight() { return _RightTube; }

    private void OnDrawGizmos()
    {
        if (!_gizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _searchRad);
    }
}