using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineCollision : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] private Transform[] _sidesOfCrystals;
    [SerializeField] private int _countLineRenderer = 1, _limitCount = 0;
    [SerializeField] private float _distance = 100.0f, _time = 0;
    public bool linkedCrystal = false;
    private bool _hittedCrystal = false;
    public Action _GrowState = delegate { };
    [SerializeField] LayerMask _layerMaskObstacles, _layerMaskCrystal;
    Vector2 posUpdate;
    [SerializeField] private Vector2[] _directions;
    private Crystal _myCrystal;

    void Start()
    {
        if (!_lineRenderer)
            _lineRenderer = GetComponent<LineRenderer>();

        if (_sidesOfCrystals != null || _sidesOfCrystals.Length > 0)
            _directions = new Vector2[_sidesOfCrystals.Length];
        _myCrystal = GetComponent<Crystal>();
        linkedCrystal = false;
        _hittedCrystal = false;
        //Debug.Log($"{gameObject.name} _layerMaskCrystal = {_layerMaskCrystal.value}. iniPos = {(Vector2)transform.position}. prevCrystal = {_myCrystal.GetPrevCrystal()}");
    }

    public void SetLight(Crystal prevCrystal, Crystal crystal_arg, Crystal nC_arg)
    {
        if (prevCrystal && !nC_arg)
        {
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);
            if (linkedCrystal)
                _myCrystal.CheckIfLastCrystal(linkedCrystal);
            return;
        }
        else
            _lineRenderer.positionCount = 0;

        _hittedCrystal = false;
        _myCrystal = crystal_arg;

        nC_arg = SetLines(prevCrystal, nC_arg);
        //Debug.Log($"{gameObject.name} count_arg: {_countLineRenderer}. _lineRenderer.positionCount: {_lineRenderer.positionCount}.");

        if (prevCrystal)
        {
            //Debug.Log($"{gameObject.name} prev is linked = {prevCrystal.GetComponent<LineCollision>().linkedCrystal}");
            _myCrystal.CheckIfLastCrystal(linkedCrystal);
            if (!linkedCrystal || prevCrystal.line.linkedCrystal == false)
            {
                _lineRenderer.positionCount = 1;
                linkedCrystal = false;
                //Debug.Log($"{gameObject.name} se apagó");
            }
        }

        if (!_hittedCrystal)
        {
            nC_arg.line.linkedCrystal = false;
            _myCrystal.CheckIfLastCrystal(linkedCrystal);
        }

        if (nC_arg && nC_arg != _myCrystal /*&& nC_arg.line.linkedCrystal*/)
            nC_arg.CallCrystal(_myCrystal);
    }

    public Crystal SetLines(Crystal prevCrystal, Crystal nC_arg)
    {
        if (prevCrystal)
            if (!linkedCrystal || prevCrystal.line.linkedCrystal == false)
            {
                _lineRenderer.positionCount = 1;
                linkedCrystal = false;
                //Debug.Log($"{gameObject.name} se apagó");
                return nC_arg;
            }

        for (int i = 0; i < _sidesOfCrystals.Length; i++)
        {
            _countLineRenderer = _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_countLineRenderer, transform.position);

            _directions[i] = (Vector2)(_sidesOfCrystals[i].position - _lineRenderer.GetPosition(_countLineRenderer));

            RaycastHit2D hit = Physics2D.Raycast(_lineRenderer.GetPosition(_countLineRenderer), _directions[i]
                , _distance, _layerMaskCrystal + _layerMaskObstacles);

            //Debug.Log($"{gameObject.name} for of index = {i}.");
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                _countLineRenderer = _lineRenderer.positionCount++;

                if (((1 << hit.collider.gameObject.layer) & _layerMaskCrystal.value) != 0 && hit.collider.GetComponent<Crystal>() != prevCrystal)
                {
                    //Debug.Log($"{gameObject.name}: No colisione con obstaculos pero si con el Crystal: {hit.collider.gameObject.name}");
                    posUpdate = (Vector2)hit.transform.position;
                    linkedCrystal = true;
                    _hittedCrystal = true;
                    if (hit.collider.GetComponent<Crystal>() != nC_arg)
                        nC_arg = hit.collider.GetComponent<Crystal>();
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
        return nC_arg;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var dir in _directions)
            Gizmos.DrawRay(transform.position, dir * _distance / 1.25f);
    }
}
