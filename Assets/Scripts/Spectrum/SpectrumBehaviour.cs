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
    [SerializeField] GameObject _spectrumVisual;
    [SerializeField] SpriteRenderer _sp;
    [SerializeField] Sprite _spectrumSp;
    [SerializeField] SpriteRenderer _cloneSp;
    [SerializeField] GameObject _cloneCollider;

    [Header("Movement Data")]
    [SerializeField] float _maxRange, _arriveRadius, _maxSpeed, _maxForce;
    Vector3 _velocity;

    [Header("Clone Data")]
    [SerializeField] float _cloneRange;
    bool prepareClone = false;
    bool alreadyClone = false;
    private void Start()
    {
        _controller = new SpectrumInput(this);
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        _controller.OnUpdate();
    }
    public void Clone()
    {
        var cloneObj = Physics2D.OverlapCircle(transform.position, _cloneRange);
        
        if (!prepareClone && !alreadyClone) 
        {
            if (cloneObj == null || cloneObj.gameObject.GetComponent<ICloneable>() == null) return;
            _currentObject = cloneObj.gameObject;
            _spectrumVisual.SetActive(true);
            var cloneSp = cloneObj.gameObject.GetComponentInChildren<SpriteRenderer>();
            _sp.sprite = cloneSp.sprite;
            _sp.color = new Color(_sp.color.r, _sp.color.g, _sp.color.b, 0.5f);
            _sp.transform.localScale = cloneSp.transform.localScale;
            var coll = cloneObj.gameObject.GetComponent<Collider2D>().GetType();
            _spectrumVisual.AddComponent(coll);
            prepareClone = true;
        }
        else
        {
            var newClone = Instantiate(_currentObject);
            newClone.transform.position = transform.position;
            Destroy(newClone, 5);
            _currentObject = newClone;
            alreadyClone = true;
            SpectrumMode();
        }
    }
    public void SpectrumMode()
    {
        Destroy(_spectrumVisual.GetComponent<Collider2D>());
        prepareClone = false;
        alreadyClone = false;
        _sp.sprite = _spectrumSp;
        _sp.transform.localScale = new Vector3(.5f, .5f, .5f);
        _sp.color = new Color(_sp.color.r, _sp.color.g, _sp.color.b, 1);
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
