using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Magnet : MonoBehaviour, IElectric
{
    Action _MagnetAction = delegate { };

    [SerializeField] Vector2 attractArea;
    [SerializeField] Vector3 offset;
    [SerializeField] float attractForce;
    [SerializeField] LayerMask _metalLayerMask;
    private void Update()
    {
        _MagnetAction();
    }
    void Attract()
    {
        var obj = Physics2D.OverlapBox(transform.position + offset, attractArea, 0, _metalLayerMask);
        if (obj != null)
        {
            var objRb = obj.GetComponent<Rigidbody2D>();
            objRb.velocity += Vector2.up * attractForce;
        }
    }
    public void TurnOn()
    {
        _MagnetAction = Attract;
    }

    public void TurnOff()
    {
        _MagnetAction = delegate { };
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + offset, attractArea);
    }

}
