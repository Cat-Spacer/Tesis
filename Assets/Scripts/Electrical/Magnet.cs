using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Magnet : MonoBehaviour, IElectric
{
    Action _MagnetAction = delegate { };

    [SerializeField] Vector2 _attractArea;
    [SerializeField] Vector3 _offset;
    [SerializeField] float _attractForce, _pow = 1f;
    [SerializeField] LayerMask _floorLayerMask;
    [SerializeField] LayerMask _metalLayerMask;
    [SerializeField] GameObject _onSprite, _offSprite, _particle;

    private void Start()
    {
        _onSprite.SetActive(false);
        _offSprite.SetActive(true);
        _particle.SetActive(false);
    }

    private void Update()
    {
        _MagnetAction();
    }
    void Attract()
    {
        var obj = Physics2D.OverlapBox(transform.position + _offset, _attractArea, 0, _metalLayerMask);
        if (obj != null)
        {
            float dist = (obj.transform.position - transform.position).magnitude;
          //  Debug.Log($"dist: {dist}");
            var objRb = obj.GetComponent<Rigidbody2D>();
            objRb.velocity += Vector2.up * (_attractForce / Mathf.Pow(dist, _pow));// Vector2.up * (attractForce/matf.elev(dist,n))
        }
    }
    public void TurnOn()
    {
        _MagnetAction = Attract;
        _onSprite.SetActive(true);
        _offSprite.SetActive(false);
        _particle.SetActive(true);
    }

    public void TurnOff()
    {
        _MagnetAction = delegate { };
        _onSprite.SetActive(false);
        _offSprite.SetActive(true);
        _particle.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + _offset, _attractArea);
    }

}
