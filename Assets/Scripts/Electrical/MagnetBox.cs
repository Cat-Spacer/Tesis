using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Collider2D))]
public class MagnetBox : MonoBehaviour
{
    [SerializeField] private float _gravity = 9.8f, _overlapOffSet = 1.1f;
    [SerializeField] private bool _useGravity = true, _playOnce = true;
    [SerializeField] private LayerMask _collLayer, _floorMask;
    [SerializeField] private SoundManager.Types _sound;
    [SerializeField] private ParticleSystem _fallParticle;
    private Collider2D _coll2D;
    [SerializeField] private int _index = 0;

    private void Start()
    {
        _coll2D = GetComponent<Collider2D>();
        if (!(_fallParticle && GetComponentInChildren<ParticleSystem>()))
            _fallParticle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        CustomGravity();
        StopMovement();
    }

    private void CustomGravity()
    {
        if (!(_useGravity && !Physics2D.OverlapBox(transform.position, _coll2D.bounds.size * _overlapOffSet, 0, _collLayer))) return;

        _playOnce = true;
        transform.position += Vector3.down * _gravity * Time.deltaTime;
    }

    private void StopMovement()
    {
        if ((!Physics2D.OverlapBox(transform.position, _coll2D.bounds.size * _overlapOffSet, 0, _collLayer) && _index <= 1) || !_playOnce) return;

        Debug.Log($"{gameObject.name} stopt");
        transform.position = transform.position;
        _playOnce = false;
        var objLayer = Physics2D.OverlapBox(transform.position, _coll2D.bounds.size, 0, _collLayer);
        if (objLayer) PlayFeedbacks(objLayer.gameObject.layer);
    }

    private void PlayFeedbacks(LayerMask layer)
    {
        SoundManager.instance.Play(_sound);
        if (_fallParticle && (_floorMask.value & (1 << layer.value)) > 0) _fallParticle.Play(); // si colisiona con algo que es floor hace particulas
        else
            Debug.Log($"Play metal crash particles");
    }

    public bool GetSetUseGravity { get { return _useGravity; } set { _useGravity = value; } }
    public int GetSetIndex { get { return _index; } set { _index = value; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size * _overlapOffSet);
    }
}
