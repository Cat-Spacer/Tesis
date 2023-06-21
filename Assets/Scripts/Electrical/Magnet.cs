using System;
using UnityEngine;

public class Magnet : MonoBehaviour, IElectric
{
    Action _MagnetAction = delegate { };

    [SerializeField] private Vector2 _attractArea;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _attractForce, _pow = 1f/*, _degrees = 0*/, _attractLimit = 1.0f;
    [SerializeField] private LayerMask _floorLayerMask, _metalLayerMask;
    [SerializeField] private GameObject _onSprite, _offSprite, _particle;
    [SerializeField] private bool _gizmos = true, _test = false, _collider = true, _attractWPhysics = true;
    [SerializeField] private MagnetBox _box = null;
    private bool _doOnce = true, _active = false;

    private void Start()
    {
        _onSprite.SetActive(false);
        _offSprite.SetActive(true);
        _particle.SetActive(false);
        _active = false;

        if (_test)
            TurnOn();
    }

    private void FixedUpdate()
    {
        _MagnetAction();
    }

    void AttractWPhysics()
    {
        if (_collider) return;
        var obj = Physics2D.OverlapBox(transform.position + _offset, _attractArea, transform.rotation.z, _metalLayerMask);
        if (!obj) return;

        if (obj.GetComponent<MetalBox>())
        {
            var metalBox = obj.GetComponent<MetalBox>();
            var boxColl = obj.GetComponent<BoxCollider2D>();
            if (metalBox.GetRigidbody.bodyType == RigidbodyType2D.Static
                && Vector2.Distance(transform.position, metalBox.transform.position) > boxColl.bounds.size.magnitude)
                metalBox.DefrostPos();
        }

        float dist = (obj.transform.position - transform.position).magnitude;
        Vector2 dir = transform.position - obj.transform.position;

        var objRb = obj.GetComponent<Rigidbody2D>();
        if (objRb)
            if (objRb.bodyType != RigidbodyType2D.Static) objRb.velocity += dir * (_attractForce / Mathf.Pow(dist, _pow));
    }

    void AttractWTransform()
    {
        if (_collider) return;
        var obj = Physics2D.OverlapBox(transform.position + _offset, _attractArea, transform.rotation.z, _metalLayerMask);
        if (!obj)
        {
            _box = null;
            return;
        }
        else
            _box = obj.GetComponent<MagnetBox>();

        if (_box && _doOnce)
        {
            _box.transform.SetParent(GameManager.Instance.GetConfig.mainGame);

            _doOnce = false;
            if (_box.GetSetUseGravity) _box.GetSetUseGravity = false;
            _box.GetSetIndex++;
        }

        if (_box.GetSetIndex > 1) return;

        float dist = (_box.transform.position - transform.position).magnitude;
        Vector3 dir = transform.position - _box.transform.position;
        _box.transform.position += dir * (_attractForce / Mathf.Pow(dist, _pow)) * Time.deltaTime;
    }

    public void TurnOn()
    {
        _onSprite.SetActive(true);
        _offSprite.SetActive(false);
        _particle.SetActive(true);
        _active = true;

        if (_attractWPhysics) _MagnetAction = AttractWPhysics;
        else _MagnetAction = AttractWTransform;
    }

    public void TurnOff()
    {
        _MagnetAction = delegate { };
        _onSprite.SetActive(false);
        _offSprite.SetActive(true);
        _particle.SetActive(false);
        _active = false;
        _doOnce = true;
        if (_box)
        {
            _box.GetSetIndex--;
            _box.GetSetUseGravity = true;
            _box = null;
        }
    }

    public bool GetActive {  get { return _active; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.GetComponent<Rigidbody2D>() || _collider)) return;
        var obj = collision.gameObject;

        if (obj.GetComponent<MetalBox>())
        {
            var metalBox = obj.GetComponent<MetalBox>();
            var boxColl = obj.GetComponent<BoxCollider2D>();
            if (metalBox.GetRigidbody.bodyType == RigidbodyType2D.Static
                && Vector2.Distance(transform.position, metalBox.transform.position) > boxColl.bounds.size.magnitude/*_attractLimit*/)
            {
                metalBox.DefrostPos();
                Debug.Log($"DEfrezeado");
            }
        }

        float dist = (obj.transform.position - transform.position).magnitude;
        Vector2 dir = transform.position - obj.transform.position;

        var objRb = obj.GetComponent<Rigidbody2D>();
        objRb.velocity += dir * (_attractForce / Mathf.Pow(dist, _pow));
    }

    private void OnDrawGizmosSelected()
    {
        if (!_gizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + _offset, _attractArea);
    }
}