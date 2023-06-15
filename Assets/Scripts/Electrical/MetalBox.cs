using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MetalBox : MonoBehaviour
{
    [SerializeField] private LayerMask _collMask/*, _playerMask*/, _floorMask;
    [SerializeField] private SoundManager.Types _sound;
    [SerializeField] private ParticleSystem _fallParticle;
    private Magnet _magnet;
    private Rigidbody2D _myRB2D = null;

    void Start()
    {
        if (!_myRB2D)
        {
            _myRB2D = GetComponent<Rigidbody2D>();
            _myRB2D.bodyType = RigidbodyType2D.Dynamic;
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

    public void FreezePos()
    {
        if (!(_myRB2D.bodyType == RigidbodyType2D.Static))
            _myRB2D.bodyType = RigidbodyType2D.Static;
    }

    public void DefrostPos()
    {
        if (!(_myRB2D.bodyType == RigidbodyType2D.Dynamic))
            _myRB2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void PlayFeedbacks(LayerMask layer)
    {
        SoundManager.instance.Play(_sound);
        if (_fallParticle && layer.value == _floorMask.value) _fallParticle.Play();
        else
            Debug.Log($"Play metal crash particles");
    }

    public Magnet GetSetMagnet { get { return _magnet; } set { _magnet = value; } }
    public Rigidbody2D GetRigidbody { get { return _myRB2D; } }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_myRB2D) return;

        if ((_collMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            PlayFeedbacks(collision.gameObject.layer);
            if (!_magnet)
                FreezePos();
            else
                DefrostPos();
        }

        if (collision.gameObject.GetComponent<Magnet>() == null) return;

        _magnet = collision.gameObject.GetComponent<Magnet>();
        if (_magnet.gameObject.layer != _collMask) _collMask += _magnet.gameObject.layer;
        if (_magnet.active)
            FreezePos();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!_myRB2D) return;

        if ((_collMask.value & (1 << collision.gameObject.layer)) > 0 && _magnet)
        {
            if (!_magnet.active)
                DefrostPos();
        }
        else if ((_collMask.value & (1 << collision.gameObject.layer)) > 0)
            DefrostPos();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_myRB2D) return;

        if ((_collMask.value & (1 << collision.gameObject.layer)) > 0 && _magnet)
        {
            if (_magnet.active)
                FreezePos();
        }
        else if ((_collMask.value & (1 << collision.gameObject.layer)) > 0)
            FreezePos();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_myRB2D) return;


        if ((_collMask.value & (1 << collision.gameObject.layer)) > 0 && _magnet)
        {
            if (!_magnet.active)
                DefrostPos();
        }
        else if ((_collMask.value & (1 << collision.gameObject.layer)) > 0)
            DefrostPos();
    }
}