using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float smoothFactor = 1.0f, _focusRad = 2.0f;
    private Hamster _hamster;

    private void Start()
    {
        _hamster = FindObjectOfType<Hamster>();
        if (_hamster) _target = _hamster.transform;
        transform.position = _target.position + _offset;
    }

    private void Update()
    {
        if (_target == null) return;

        if (_hamster.Generator)
        {
            if (Vector3.Distance(_target.position, _hamster.Generator.transform.position) < _focusRad)
                transform.position = Vector3.Lerp((Vector2)transform.position,
                    (Vector2)((_hamster.Generator.transform.position + _target.position) / 2.0f),
                    smoothFactor * Time.deltaTime) + _offset;
            else
                transform.position = _target.position + _offset;
        }
        else
            transform.position = _target.position + _offset;
    }
}