using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PressurePlate : MonoBehaviour
{
    Action _PressurePlateAction = delegate { };
    [SerializeField] float speedUp, speedDown;
    [SerializeField] Transform _topPoint, _downPoint;
  
    void Update()
    {
        _PressurePlateAction();
    }

    void OnPress()
    {
        if (Vector2.Distance(_downPoint.position, transform.position) > .1) transform.Translate(0, -speedDown, 0);
        else _PressurePlateAction = delegate { };       
    }
    void OnStop()
    {
        if (Vector2.Distance(_topPoint.position, transform.position) > .1) transform.Translate(0, speedUp, 0);
        else _PressurePlateAction = delegate { };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _PressurePlateAction = OnPress;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _PressurePlateAction = OnStop;
    }
}
