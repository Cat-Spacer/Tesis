using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumBehaviour : MonoBehaviour
{
    SpectrumInput _controller;
    Rigidbody2D _rb;
    public Transform _player;

    [Header("Sprite")]
    [SerializeField] GameObject _currentObject;

    [Header("Movement Data")]
    [SerializeField] float _maxRange, _arriveRadius, _maxSpeed, _maxForce;
    Vector3 _velocity;

    [Header("Clone Data")]
    [SerializeField] float _cloneRange;
    bool alreadyCloned = false;
    private void Start()
    {
        _controller = new SpectrumInput(this);
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _controller.OnUpdate();
    }
    public void Clone()
    {
        if (alreadyCloned) return;
        var cloneObj = Physics2D.OverlapCircle(transform.position, _cloneRange);
        if (cloneObj != null && cloneObj.gameObject.GetComponent<ICloneable>() != null)
        {
            var newClone = Instantiate(cloneObj.gameObject, transform);
            newClone.transform.position = transform.position;
            _currentObject = newClone;
            alreadyCloned = true;
        }
    }
    public void SpectrumMode()
    {
        alreadyCloned = false;
        Destroy(_currentObject);
    }
    public void Move(Vector3 dir)
    {
        ApplyForce(Arrive(dir));
        _rb.velocity = _velocity;
    }
    Vector3 Arrive(Vector3 dir)
    {
        Vector3 desired = dir - transform.position;
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
        Gizmos.DrawWireSphere(transform.position, _cloneRange);
    }
}
