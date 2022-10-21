using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LineCollision : MonoBehaviour
{
    List<Vector2> _colliderList = new List<Vector2>();
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] private Transform[] _sidesOfCrystals;
    [SerializeField] private int _count = 0, _limitCount = 0;
    [SerializeField] private float _distance = 100.0f, _time = 0;
    public Action _GrowState = delegate { };
    [SerializeField] LayerMask _layerMaskObstacles, _layerMaskCrystal;
    Vector2 posUpdate, nextPos, obstaclePosition;
    Vector2[] _directions;
    Crystal _lastCrystal;
    void Start()
    {
        if (!_lineRenderer)
            _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, transform.position);
        Debug.Log($"{gameObject.name} _layerMaskObstacles.value = {_layerMaskObstacles.value} && iniPos = {(Vector2)transform.position}");
        // _colliderList = _lineCollider.GetPoints();
        if (_sidesOfCrystals != null || _sidesOfCrystals.Length > 0)
            _directions = new Vector2[_sidesOfCrystals.Length];
    }
    // Update is called once per frame
    void Update()
    {
        _GrowState();
    }

    public void AddPoint(int num_arg)
    {
        //_lineRenderer.positionCount = num_arg + 1;
        _lineRenderer.positionCount++;
        //  _lineCollider.points[].a
    }

    Vector2 nullObj = new Vector2(3, 3);//                 added  ↓  added
    public void SetLight(Vector2 nextPos_arg, int count_arg, int limit, Crystal crystal_arg, Crystal nC_arg)
    {
        if (posUpdate == nextPos_arg) return;
        _lastCrystal = crystal_arg;
        _count = count_arg;

        _lineRenderer.positionCount = count_arg + 1;
        //_lineRenderer.SetPosition(_count, posUpdate);
        Debug.Log($"{gameObject.name} count_arg: {count_arg}. nextPos_arg: {nextPos_arg}. _lineRenderer.positionCount: {_lineRenderer.positionCount}.");
        //_directions = (nextPos_arg - (Vector2)_lineRenderer.GetPosition(count_arg - 1));// esta verga estaba al revés

        //RaycastHit2D hit = Physics2D.Raycast(_lineRenderer.GetPosition(count_arg - 1), _directions, _directions.magnitude, _layerMaskObstacles);
        //Debug.DrawRay(transform.position, _directions, Color.green);

        foreach (var side in _sidesOfCrystals)
        {
            _directions[_sidesOfCrystals.Count()] = (Vector2)(side.position - _lineRenderer.GetPosition(count_arg - 1));// esta verga estaba al revés

            RaycastHit2D hit = Physics2D.Raycast(_lineRenderer.GetPosition(count_arg - 1), _directions[_sidesOfCrystals.Count()], _distance, _layerMaskObstacles);
            Debug.DrawRay(transform.position, _directions[_sidesOfCrystals.Count()], Color.green);

            Debug.Log($"{gameObject.name} direction: {_directions}. count_arg - 1 = {count_arg - 1}. hit : {(Vector2)hit.point}. direction.magnitude = {_directions[_sidesOfCrystals.Count()].magnitude}");
            Debug.Log($"{gameObject.name} RaycastHit: {hit.collider}.");
            //Physics2D.Raycast(_lineRenderer.GetPosition(count_arg - 1), direction, direction.magnitude, _layerMaskObstacles)
            if (hit.collider) //hit.transform.gameObject.layer == _layerMaskObstacles
            {
                Debug.Log($"{gameObject.name} hit.point = {(Vector2)hit.point}");
                if (hit.transform)
                    Debug.Log($"hit del {gameObject.name} colisiono con el obstaculo: {hit.transform.gameObject.name}");
                nullObj = hit.point;
                posUpdate = nullObj;
                _lineRenderer.positionCount--;
            }
            else
            {
                Debug.Log($"{gameObject.name}: No colisione con obstaculos");
                posUpdate = nextPos_arg;

                //_lineRenderer.SetPosition(0, transform.position);
                if (posUpdate != Vector2.zero)
                    _lineRenderer.SetPosition(_count, posUpdate);
                else
                    _lineRenderer.positionCount--;
                _lastCrystal.CheckCrystal();
                break;
            }
        }

        Debug.Log(gameObject.name + " posUpdate " + posUpdate);
        /*_lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(_count, posUpdate);
        _lastCrystal.CheckCrystal();*/
        if (nC_arg != null && posUpdate == nextPos_arg)
            nC_arg.CallCrystal();
    }
    #region oldMetods
    /* int count;
    void StartGrow()
    {

        _GrowState = WhileGrow;
    }
    void WhileGrow()
    {

        if (InSightObstacle())
            posUpdate = obstaclePosition;
        else if (!InSightObstacle())
            posUpdate = nextPos;

        Debug.Log("posUpdate (WhileGrow)" + posUpdate);

        _lineRenderer.SetPosition(0, transform.localPosition);
        _lineRenderer.SetPosition(_count, posUpdate);
        _lastCrystal.CheckCrystal();
        _lastCrystal.CallCrystal();
        _GrowState = delegate { };
        //InSightCrystal().GetComponent<Crystal>().CallCrystal();

        // _GrowState = delegate { };

    }

    public bool InSightObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, _directions[].magnitude, ~_layerMaskObstacles);

        if (hit.collider != null)
        {
            Debug.Log(hit.point);
            obstaclePosition = hit.point;
        }

        if (Physics2D.Raycast(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), nextPos, _directions.magnitude, ~_layerMaskObstacles)) return true;
        else return false;
    }
    public Transform InSightCrystal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, _directions.magnitude, ~_layerMaskCrystal);

        if (hit.collider != null)
        {
            Debug.Log(hit.point);
            return (hit.transform);
        }
        else return default;
    }*/
    #endregion
}
