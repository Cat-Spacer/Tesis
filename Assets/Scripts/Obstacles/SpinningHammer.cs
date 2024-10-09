using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpinningHammer : MonoBehaviour
{
    [SerializeField] private Transform rotationPos;
    [SerializeField] private float rotationSpeed;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }
}
