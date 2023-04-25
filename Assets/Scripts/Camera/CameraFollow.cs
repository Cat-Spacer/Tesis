using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        if (_target == null) return;
        transform.position = _target.position + _offset;
    }
    void CheckBounds()
    {
        if (transform.position.y > 5) transform.position = new Vector2(transform.position.x, transform.position.y);
        if (transform.position.y < -5) transform.position = new Vector2(transform.position.x, transform.position.y);
        if (transform.position.x < -5) transform.position = new Vector2(5, transform.position.y);
        if (transform.position.x > 5) transform.position = new Vector2(-5, transform.position.y);
    }
}
