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
    [SerializeField] private bool _gizmos = true, _test = false, _collider = true;
    [HideInInspector] public bool active = false;

    private void Start()
    {
        _onSprite.SetActive(false);
        _offSprite.SetActive(true);
        _particle.SetActive(false);
        active = false;

        if (_test)
            TurnOn();
    }

    private void FixedUpdate()
    {
        _MagnetAction();
    }

    void Attract()
    {
        if (_collider) return;
        var obj = Physics2D.OverlapBox((transform.position + _offset), _attractArea, transform.rotation.z, _metalLayerMask);
        if (obj != null)
        {
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
            if (objRb.bodyType != RigidbodyType2D.Static) objRb.velocity += dir * (_attractForce / Mathf.Pow(dist, _pow));
        }
    }

    public void TurnOn()
    {
        _MagnetAction = Attract;
        _onSprite.SetActive(true);
        _offSprite.SetActive(false);
        _particle.SetActive(true);
        active = true;
    }

    public void TurnOff()
    {
        _MagnetAction = delegate { };
        _onSprite.SetActive(false);
        _offSprite.SetActive(true);
        _particle.SetActive(false);
        active = false;
    }

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
        //Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(transform.position + _offset, _attractArea);
    }
}