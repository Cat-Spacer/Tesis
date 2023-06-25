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
            transform.position = Vector3.Lerp(transform.position, _hamster.Generator.transform.position, smoothFactor * Time.deltaTime);
        else
            transform.position = _target.position + _offset;
    }
}