using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MetalBox : MonoBehaviour
{
    [SerializeField] private LayerMask _collMask, _platformMask, _floorMask;
    [SerializeField] private SoundManager.Types _sound;
    [SerializeField] private ParticleSystem _fallParticle;
    [SerializeField] private Magnet _magnet;
    private Rigidbody2D _myRB2D = null;
    private bool _isColl;
    private Rigidbody2D _objToFollow;

    void Start()
    {
        if (!_myRB2D)
        {
            _myRB2D = GetComponent<Rigidbody2D>();
            DefrostPos();
        }
        if (!(_fallParticle && GetComponentInChildren<ParticleSystem>()))
            _fallParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!_magnet) return;
        if (!_magnet.active)
            DefrostPos();
    }

    private void FixedUpdate()
    {
        if (_isColl)
            MoveWithColl();
    }

    private void MoveWithColl()
    {
        if (!_myRB2D) return;
        _myRB2D.velocity = _objToFollow.velocity;
    }

    public void FreezePos()
    {
        //Debug.Log($"Frezeado");
        if (_myRB2D.bodyType != RigidbodyType2D.Static)
        {
            _myRB2D.velocity = Vector2.zero;
            _myRB2D.bodyType = RigidbodyType2D.Static;
        }
    }

    public void DefrostPos()
    {
        //Debug.Log($"DEfrezeado");
        if (_myRB2D.bodyType != RigidbodyType2D.Dynamic)
        {
            _myRB2D.bodyType = RigidbodyType2D.Dynamic;
            _myRB2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void PlayFeedbacks(LayerMask layer)
    {
        SoundManager.instance.Play(_sound);
        if (_fallParticle && (_floorMask.value & (1 << layer.value)) > 0) _fallParticle.Play(); // si colisiona con algo que es floor hace particulas
        else
            Debug.Log($"Play metal crash particles");
    }

    public Magnet GetSetMagnet { get { return _magnet; } set { _magnet = value; } }
    public Rigidbody2D GetRigidbody { get { return _myRB2D; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_myRB2D) return;
        //Debug.Log($"{gameObject.name} colisionó con {collision.gameObject.name} con layer {collision.gameObject.layer}");

        if ((_platformMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log($"{gameObject.name} colisionó con {collision.gameObject.name}");
            _objToFollow = collision.GetComponent<Rigidbody2D>();
            _isColl = true;
        }

        if ((_collMask.value & (1 << collision.gameObject.layer)) != 0 && !collision.gameObject.GetComponent<Magnet>())
        {
            PlayFeedbacks(collision.gameObject.layer);
            FreezePos();
        }

        if (!collision.gameObject.GetComponent<Magnet>()) return;
        _magnet = collision.gameObject.GetComponent<Magnet>();
        if (!((_collMask.value & (1 << collision.gameObject.layer)) != 0)) _collMask += _magnet.gameObject.layer;
        if (_magnet.active)
            FreezePos();
        else
            DefrostPos();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_myRB2D) return;
        _isColl = false;

        /*if ((_collMask.value & (1 << collision.gameObject.layer)) > 0 && _magnet)
        {
            if (!_magnet.active)
                DefrostPos();
        }
        else if ((_collMask.value & (1 << collision.gameObject.layer)) > 0)
            DefrostPos();*/
    }
}