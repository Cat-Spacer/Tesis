using UnityEngine;

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
        if (!(_useGravity && !CollWLayer())) return;
        transform.SetParent(GameManager.Instance.GetConfig.mainGame);

        if (_index > 0)
            _index--;

        _playOnce = true;
        transform.position += Vector3.down * _gravity * Time.deltaTime;
    }

    private void ParentToObject()
    {
        if (!CollWLayer()) return;
        var platfomr = Physics2D.OverlapBox(transform.position, _coll2D.bounds.size * _overlapOffSet, 0, _collLayer).GetComponent<MoveOnCollision>();
        if (platfomr) transform.SetParent(platfomr.transform);
    }

    private void StopMovement()
    {
        if (!(CollWLayer() && _index <= 1) || !_playOnce) return;

        ParentToObject();

        if (_playOnce) _playOnce = false;

        var objLayer = Physics2D.OverlapBox(transform.position, _coll2D.bounds.size, 0, _collLayer);
        if (objLayer) PlayFeedbacks(objLayer.gameObject.layer);
    }

    public bool CollWLayer()
    {
        return Physics2D.OverlapBox(transform.position, _coll2D.bounds.size * _overlapOffSet, 0, _collLayer);
    }

    private void PlayFeedbacks(LayerMask layer)
    {
        SoundManager.instance.Play(_sound);
        if (_fallParticle && (_floorMask.value & (1 << layer.value)) > 0) _fallParticle.Play(); // si colisiona con algo que es floor hace particulas
        else
            Debug.Log($"{gameObject.name} Played metal crash particles");
    }

    public bool GetSetUseGravity { get { return _useGravity; } set { _useGravity = value; } }
    public int GetSetIndex { get { return _index; } set { _index = value; } }
}