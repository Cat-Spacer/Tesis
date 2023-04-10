using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumBehaviour : MonoBehaviour
{
    SpectrumInput _controller;
    SpectrumInventory _inventory;
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
        _inventory = GetComponent<SpectrumInventory>();
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        _controller.OnUpdate();
    }
    public void Clone()
    {
        if (!prepareClone && !alreadyClone)
        {
            var obj = Physics2D.OverlapCircle(transform.position, _cloneRange);
            if (obj == null) return;
            var cloneableObj = obj.GetComponent<Cloneable>();
            if (cloneableObj == null || cloneableObj.GetBool()) return;

            _currentObject = obj.gameObject;
            _spectrumVisual.SetActive(true);
            var cloneSp = obj.gameObject.GetComponentInChildren<SpriteRenderer>();
            _sp.sprite = cloneSp.sprite;
            _sp.color = new Color(_sp.color.r, _sp.color.g, _sp.color.b, 0.5f);
            _sp.transform.localScale = cloneSp.transform.localScale;
            var coll = obj.gameObject.GetComponent<Collider2D>().GetType();
            _spectrumVisual.AddComponent(coll);
            prepareClone = true;
        }
        else
        {
            var newClone = Instantiate(_currentObject);
            newClone.transform.position = transform.position;
            newClone.GetComponent<Cloneable>().Clone(true);
            Destroy(newClone, 5);
            _currentObject = newClone;
            alreadyClone = true;
            SpectrumMode();
        }
    }
    public void CloneFromInventory(int slot)
    {
        if (_inventory.inventory[slot] == null) return;

        var obj = _inventory.GetObject(slot);
        _currentObject = obj.gameObject;
        _spectrumVisual.SetActive(true);
        var cloneSp = obj.gameObject.GetComponentInChildren<SpriteRenderer>();
        _sp.sprite = cloneSp.sprite;
        _sp.color = new Color(_sp.color.r, _sp.color.g, _sp.color.b, 0.5f);
        _sp.transform.localScale = cloneSp.transform.localScale;
        var coll = obj.gameObject.GetComponent<Collider2D>().GetType();
        _spectrumVisual.AddComponent(coll);
        prepareClone = true;       
    }
    public void SaveObject()
    {
        var obj = Physics2D.OverlapCircle(transform.position, _cloneRange);
        if (obj == null) return;
        var cloneableObj = obj.GetComponent<Cloneable>();
        if (cloneableObj == null || cloneableObj.GetBool()) return;

        for (int i = 0; i < _inventory.inventory.Length; i++)
        {
            if (_inventory.inventory[i] == obj.gameObject) return;
            if (_inventory.inventory.Length - 1 == i)
            {
                _inventory.OnInventoryAdd(obj.gameObject, cloneableObj.GetSprite());
            }
        }   
    }
    public void SpectrumMode()
    {
        prepareClone = false;
        alreadyClone = false;
        _sp.sprite = _spectrumSp;
        _sp.transform.localScale = new Vector3(.5f, .5f, .5f);
        _sp.color = new Color(_sp.color.r, _sp.color.g, _sp.color.b, 1);
        Destroy(_spectrumVisual.GetComponent<Collider2D>());
    }

    #region Movement
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
    void ApplyForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_player.position, _maxRange);
        Gizmos.DrawWireSphere(transform.position, _cloneRange);
    }
}
