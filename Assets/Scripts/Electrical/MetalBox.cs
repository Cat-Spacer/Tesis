using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MetalBox : MonoBehaviour
{
    // [SerializeField] private LayerMask _collMask, _floorMask;
    // [SerializeField] private SoundManager.Types _sound;
    // [SerializeField] private ParticleSystem _fallParticle;
    // [SerializeField] private Magnet _magnet;
    // private Rigidbody2D _myRB2D = null;
    // private bool _onPlatform = false;
    //
    // void Start()
    // {
    //     _onPlatform = false;
    //     if (!_myRB2D)
    //     {
    //         _myRB2D = GetComponent<Rigidbody2D>();
    //         DefrostPos();
    //     }
    //     if (!(_fallParticle && GetComponentInChildren<ParticleSystem>()))
    //         _fallParticle = GetComponentInChildren<ParticleSystem>();
    // }
    //
    // private void Update()
    // {
    //     if (!_magnet) return;
    //     if (!_magnet.GetActive)
    //         DefrostPos();
    // }
    //
    // public void FreezePos()
    // {
    //     //Debug.Log($"Frezeado");
    //     if (_myRB2D.bodyType != RigidbodyType2D.Static)
    //     {
    //         _myRB2D.velocity = Vector2.zero;
    //         _myRB2D.bodyType = RigidbodyType2D.Static;
    //     }
    // }
    //
    // public void DefrostPos()
    // {
    //     //Debug.Log($"DEfrezeado");
    //     if (_myRB2D.bodyType != RigidbodyType2D.Dynamic)
    //     {
    //         _myRB2D.bodyType = RigidbodyType2D.Dynamic;
    //         _myRB2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    //     }
    // }
    //
    // private void PlayFeedbacks(LayerMask layer)
    // {
    //     SoundManager.instance.Play(_sound);
    //     if (_fallParticle && (_floorMask.value & (1 << layer.value)) > 0) _fallParticle.Play(); // si colisiona con algo que es floor hace particulas
    //     else
    //         Debug.Log($"Play metal crash particles");
    // }
    //
    // public Magnet GetSetMagnet { get { return _magnet; } set { _magnet = value; } }
    // public Rigidbody2D GetRigidbody { get { return _myRB2D; } }
    //
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (!_myRB2D) return;
    //     if(collision.gameObject.GetComponent<MoveOnCollision>()) _onPlatform = true;
    //     if ((_collMask.value & (1 << collision.gameObject.layer)) != 0 && !collision.gameObject.GetComponent<Magnet>() && !_onPlatform)
    //     {
    //         PlayFeedbacks(collision.gameObject.layer);
    //         FreezePos();
    //     }
    //
    //     if (!collision.gameObject.GetComponent<Magnet>()) return;
    //     _magnet = collision.gameObject.GetComponent<Magnet>();
    //     if (!((_collMask.value & (1 << collision.gameObject.layer)) != 0)) _collMask += _magnet.gameObject.layer;
    //     if (_magnet.GetActive)
    //         FreezePos();
    //     else
    //         DefrostPos();
    // }
    //
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (!_myRB2D) return;
    //     if (collision.gameObject.GetComponent<MoveOnCollision>()) _onPlatform = false;
    // }
}