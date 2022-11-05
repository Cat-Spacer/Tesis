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
    [SerializeField] private int _countLineRenderer = 1, _limitCount = 0;
    [SerializeField] private float _distance = 100.0f, _time = 0;
    public bool linkedCrystal = false;
    public Action _GrowState = delegate { };
    [SerializeField] LayerMask _layerMaskObstacles, _layerMaskCrystal;
    Vector2 posUpdate, nextPos, obstaclePosition/*, nullObj = new Vector2()*/;
    [SerializeField] private Vector2[] _directions;
    private Crystal _myCrystal;
    void Start()
    {
        if (!_lineRenderer)
            _lineRenderer = GetComponent<LineRenderer>();
        //_lineRenderer.SetPosition(0, transform.position);

        if (_sidesOfCrystals != null || _sidesOfCrystals.Length > 0)
            _directions = new Vector2[_sidesOfCrystals.Length];
        _myCrystal = GetComponent<Crystal>();
        linkedCrystal = false;
        //Debug.Log($"{gameObject.name} _layerMaskCrystal = {_layerMaskCrystal.value}. iniPos = {(Vector2)transform.position}. prevCrystal = {_myCrystal.GetPrevCrystal()}");
    }

    void Update()
    {
        //_GrowState();
        /*if (linkedCrystal && _myCrystal != null)
            SetLight(_myCrystal.GetPrevCrystal(), _myCrystal, _myCrystal.GetNextCrystal());*/
    }

    public void SetLight(Crystal prevCrystal, Crystal crystal_arg, Crystal nC_arg)
    {
        if (prevCrystal && !nC_arg)
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);
            return;
        }
        else
            _lineRenderer.positionCount = 0;
        //linkedCrystal = false;
        _myCrystal = crystal_arg;

        SetLines(prevCrystal, nC_arg);
        //Debug.Log($"{gameObject.name} count_arg: {_countLineRenderer}. _lineRenderer.positionCount: {_lineRenderer.positionCount}.");

        if (prevCrystal)
        {
            //Debug.Log($"{gameObject.name} prev is linked = {prevCrystal.GetComponent<LineCollision>().linkedCrystal}");
            if (!linkedCrystal || prevCrystal.line.linkedCrystal == false)
            {
                _lineRenderer.positionCount = 1;
                linkedCrystal = false;
                //Debug.Log($"{gameObject.name} se apagó");
            }
            _myCrystal.CheckIfLastCrystal(linkedCrystal);
        }
        if (nC_arg && nC_arg != _myCrystal && nC_arg.line.linkedCrystal)
            nC_arg.CallCrystal(_myCrystal);
    }

    private void SetLines(Crystal prevCrystal, Crystal nC_arg)
    {
        //_countLineRenderer = _lineRenderer.positionCount ++;
        for (int i = 0; i < _sidesOfCrystals.Length; i++)
        {
            _countLineRenderer = _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_countLineRenderer, transform.position);

            _directions[i] = (Vector2)(_sidesOfCrystals[i].position - _lineRenderer.GetPosition(_countLineRenderer));

            RaycastHit2D hit = Physics2D.Raycast(_lineRenderer.GetPosition(_countLineRenderer), _directions[i]
                , _distance, _layerMaskCrystal + _layerMaskObstacles);

            Debug.Log($"{gameObject.name} for of index = {i}.");
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                _countLineRenderer = _lineRenderer.positionCount++;

                if (((1 << hit.collider.gameObject.layer) & _layerMaskCrystal.value) != 0 && hit.collider.GetComponent<Crystal>() != prevCrystal)
                {
                    Debug.Log($"{gameObject.name}: No colisione con obstaculos pero si con el Crystal: {hit.collider.gameObject.name}");
                    posUpdate = (Vector2)hit.transform.position;
                    linkedCrystal = true;
                    nC_arg.line.linkedCrystal = true;
                }
                else
                    posUpdate = (Vector2)hit.point;

                _lineRenderer.SetPosition(_countLineRenderer, posUpdate);

                #region Test
                //Debug.Log($"{gameObject.name} direction: {_directions[i]}. _lineRenderer pos = {_lineRenderer.GetPosition(_countLineRenderer - 1)}. hit : {(Vector2)hit.point}. _sidesOfCrystals pos = {_sidesOfCrystals[i].position}");
                //Debug.Log($"{gameObject.name} posUpdate = {posUpdate}. HittedCrystal = {hit.collider.GetComponent<Crystal>()} compared to prevCrystal = {prevCrystal}");
                //Debug.Log($"{gameObject.name} RaycastHit: {hit.collider}. RaycastHit layer: {hit.collider.gameObject.layer}.");

                /*if (((1 << hit.collider.gameObject.layer) & _layerMaskObstacles.value) != 0)
                {
                    Debug.Log($"{gameObject.name} hit.point = {(Vector2)hit.point}");
                    if (hit.transform)
                        Debug.Log($"hit del {gameObject.name} colisiono con el obstaculo: {hit.transform.gameObject.name}");
                }
                else */
                #endregion
            }
            else
                _countLineRenderer = _lineRenderer.positionCount--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var dir in _directions)
            Gizmos.DrawRay(transform.position, dir * _distance / 1.25f);
    }
    #region oldMethods
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
