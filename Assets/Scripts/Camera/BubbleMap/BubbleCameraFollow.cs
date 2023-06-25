using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float smoothFactor = 1.0f;
    private Hamster _hamster;

    private void Start()
    {
        _hamster = FindObjectOfType<Hamster>();
        _target = _hamster.transform;
    }

    private void Update()
    {
        if (_target == null) return;

        if (_hamster.Generator)
            transform.position = Vector3.Lerp((Vector2)transform.position,
                (Vector2)(_hamster.Generator.transform.position + _hamster.Generator.transform.position.normalized * 0.25f),
                smoothFactor * Time.deltaTime) + _offset;
        else
            transform.position = _target.position + _offset;
    }
}