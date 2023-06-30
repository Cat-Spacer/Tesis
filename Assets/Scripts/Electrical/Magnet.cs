using System;
using UnityEngine;

public class Magnet : MonoBehaviour, IElectric, IMouseOver
{
    Action _MagnetAction = delegate { };
    [SerializeField] private Vector2 _attractArea, _magnetArea;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _attractForce, _pow = 1f, _attractLimit = 1.0f;
    [SerializeField] private LayerMask _floorLayerMask, _metalLayerMask;
    [SerializeField] private GameObject _onSprite, _offSprite, _particle, _area, _connectionSource;
    [SerializeField] private bool _gizmos = true;
    [SerializeField] private bool _isOn = false;
    [SerializeField] private bool _collider = true, _attractWPhysics = true;
    [SerializeField] private MagnetBox _box = null;
    private bool _doOnce = true, _active = false;
    private bool firstCall = true;
    private LineRenderer _myLineConnection;
    private IGenerator _myGen;
    [SerializeField] private bool _startingBool = false;

    [SerializeField]private SpriteRenderer _myMat;
    [SerializeField] private float _speed;
    [SerializeField] private float _attractAreaOffset;
    private void Start()
    {
        FirstCall();
        EventManager.Instance.Subscribe("PlayerDeath", ResetPosition);
        _myMat = _area.GetComponent<SpriteRenderer>();
        if (firstCall) _attractAreaOffset = -1;
        else _attractAreaOffset = 0;
    }

    private void FixedUpdate()
    {
        //_MagnetAction();
    }

    private void Update()
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
            if (_box)
            {
                _box.GetSetIndex--;
                if (_box.GetSetIndex < 1)
                {
                    if (_box.GetOnGO && _box.GetOffGO)
                    {
                        _box.GetOnGO.SetActive(false);
                        _box.GetOffGO.SetActive(true);
                    }
                    _box.GetSetUseGravity = true;
                }
            }
            _box = null;
            return;
        }
        else
            _box = obj.GetComponent<MagnetBox>();

        if (_doOnce)
        {
            _box.transform.SetParent(GameManager.Instance.GetConfig.mainGame);
            if (_box.GetOnGO && _box.GetOffGO)
            {
                _box.GetOnGO.SetActive(true);
                _box.GetOffGO.SetActive(false);
            }
            _doOnce = false;
            if (_box.GetSetUseGravity) _box.GetSetUseGravity = false;
            _box.GetSetIndex++;
        }

        if (Physics2D.OverlapBox(transform.position, _magnetArea, transform.rotation.z, _metalLayerMask)) return;
        if (_box.GetSetIndex > 1) return;

        float dist = (_box.transform.position - transform.position).magnitude;
        Vector3 dir = transform.position - _box.transform.position;
        _box.transform.position += dir * (_attractForce / Mathf.Pow(dist, _pow)) * Time.deltaTime;
    }

    public void TurnOn()
    {
        if (!_isOn)
        {
            _onSprite.SetActive(true);
            _offSprite.SetActive(false);
            _particle.SetActive(true);
            _area.SetActive(true);
            _active = true;
            _isOn = true;
            if (_attractWPhysics) _MagnetAction = AttractWPhysics;
            else _MagnetAction = AttractWTransform;
            _MagnetAction += AttrackAreaOn;
        }
        else
        {
            TurnOff();
        }
    }

    public void AttrackAreaOn()
    {
        Debug.Log("on");
        _attractAreaOffset += Time.deltaTime * _speed;
        if (_attractAreaOffset <= 0)
        {
            _myMat.material.SetFloat("_offset", _attractAreaOffset);   
        }
        else
        {
            _attractAreaOffset = 0;
            _MagnetAction -= AttrackAreaOn;
        }
    }
    public void AttrackAreaOff()
    {
        _attractAreaOffset -= Time.deltaTime * _speed;
        if (_attractAreaOffset >= -1)
        {
            _myMat.material.SetFloat("_offset", _attractAreaOffset);   
        }
        else
        {
            _attractAreaOffset = -1;
            _MagnetAction = delegate {  };
        }
    }
    public void TurnOff()
    {
        if (_isOn)
        {
            _MagnetAction = AttrackAreaOff;
            _onSprite.SetActive(false);
            _offSprite.SetActive(true);
            _particle.SetActive(false);
            _active = false;
            _doOnce = true;
            _isOn = false;
            if (_box)
            {
                _box.GetSetIndex--;
                if (_box.GetSetIndex < 1) _box.GetSetUseGravity = true;
                if (_box.GetOnGO && _box.GetOffGO)
                {
                    _box.GetOnGO.SetActive(false);
                    _box.GetOffGO.SetActive(true);
                }
                _box = null;
            }
        }
        else
        {
            TurnOn();
        }
    }

    void FirstCall()
    {
        if (_isOn)
        {
            _onSprite.SetActive(true);
            _offSprite.SetActive(false);
            _particle.SetActive(true);
            _area.SetActive(true);
            _active = true;
            if (_attractWPhysics) _MagnetAction = AttractWPhysics;
            else _MagnetAction = AttractWTransform;
            _MagnetAction += AttrackAreaOn;
        }
        else
        {
            _MagnetAction = AttrackAreaOff;
            _onSprite.SetActive(false);
            _offSprite.SetActive(true);
            _particle.SetActive(false);
            _area.SetActive(false);
            _active = false;
            _doOnce = true;
            if (_box)
            {
                _box.GetSetIndex--;
                if (_box.GetSetIndex < 1) _box.GetSetUseGravity = true;
                if (_box.GetOnGO && _box.GetOffGO)
                {
                    _box.GetOnGO.SetActive(false);
                    _box.GetOffGO.SetActive(true);
                }
                _box = null;
            }
        }
    }

    public Transform ConnectionSource()
    {
        return _connectionSource.transform;
    }

    public void SetGenerator(IGenerator gen, LineRenderer line)
    {
        _myGen = gen;
        _myLineConnection = line;
    }

    public bool GetActive { get { return _active; } }

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
                metalBox.DefrostPos();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _magnetArea);
    }

    public void MouseOver()
    {
        _myGen.ShowLineConnection(_myLineConnection);
    }

    public void MouseExit()
    {
        _myGen.NotShowLineConnection(_myLineConnection);
    }

    public void Interact() { }

    void ResetPosition(params object[] param)
    {
        if (_startingBool) _isOn = true;
        else _isOn = false;
        FirstCall();
    }
}