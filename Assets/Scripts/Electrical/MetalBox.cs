using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MetalBox : MonoBehaviour
{
    [SerializeField] private LayerMask _collMask/*, _playerMask*/, _floorMask;
    [SerializeField] private SoundManager.Types _sound;
    [SerializeField] private ParticleSystem _fallParticle;
    [SerializeField] private Magnet _magnet;
    private Rigidbody2D _myRB2D = null;

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
        {
            DefrostPos();
            Debug.Log($"DEfrezeado");
        }
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

        if ((_collMask.value & (1 << collision.gameObject.layer)) > 0 && !collision.gameObject.GetComponent<Magnet>())
        {
            PlayFeedbacks(collision.gameObject.layer);
            FreezePos();
            Debug.Log($"Frezeado por {collision.gameObject.name}");
        }

        if (!collision.gameObject.GetComponent<Magnet>()) return;
        _magnet = collision.gameObject.GetComponent<Magnet>();
        if (!((_collMask.value & (1 << collision.gameObject.layer)) > 0)) _collMask += _magnet.gameObject.layer;
        if (_magnet.active)
            FreezePos();
        else
            DefrostPos();
        Debug.Log($"Frezeado");
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_myRB2D) return;


        if ((_collMask.value & (1 << collision.gameObject.layer)) > 0 && _magnet)
        {
            if (!_magnet.active)
                DefrostPos();
        }
        else if ((_collMask.value & (1 << collision.gameObject.layer)) > 0)
            DefrostPos();
    }*/
}