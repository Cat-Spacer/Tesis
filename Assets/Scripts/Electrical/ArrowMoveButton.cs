using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TubeActions
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class ArrowMoveButton : MonoBehaviour
{
    [SerializeField] private Tube _tube;
    [SerializeField] private TubeActions _tubeActions;

    void Start()
    {
        if (!_tube)
            _tube = GetComponentInParent<Tube>();
    }

    private void EjecuteOrder66()
    {
        if (!_tube) return;
        Debug.Log($"{gameObject.name}: _tubeActions = {_tubeActions}");

        if (_tubeActions == TubeActions.Up) _tube.GoUp();
        if (_tubeActions == TubeActions.Down) _tube.GoDown();
        if (_tubeActions == TubeActions.Left) _tube.GoLeft();
        if (_tubeActions == TubeActions.Right) _tube.GoRight();
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            EjecuteOrder66();
    }
}
