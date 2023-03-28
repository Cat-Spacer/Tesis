using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumBehaviour : MonoBehaviour
{
    SpectrumInput _input;
    Rigidbody2D _rb;
    public Transform _player;

    [Header("DATA")]
    [SerializeField] float _maxRange, _arriveRadius, _maxSpeed, _maxForce;
    Vector3 _velocity;

    private void Start()
    {
        //_player = GameManager.Instance.GetPlayer().transform;
        _input = GetComponent<SpectrumInput>();
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        ApplyForce(Arrive());
        _rb.velocity = _velocity;
        //Position();
    }

    Vector3 Arrive()
    {
        Vector3 desired = _input.targetPosition - transform.position;
        if (desired.magnitude < _arriveRadius)
        {
            float speed = _maxSpeed * (desired.magnitude / _arriveRadius);
            desired.Normalize();
            desired *= speed;
        }
        else
        {
            desired.Normalize();
            desired *= _maxSpeed;
        }
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
    void Position()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.position;
        targetPos.z = 0;
        if (targetPos.magnitude > _maxRange)
        {
            Vector3 dir = targetPos + _player.position;
            dir.Normalize();
            transform.position = Vector3.ClampMagnitude(dir, _maxRange);
        }
    }
    void ApplyForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_player.position, _maxRange);
    }
}
