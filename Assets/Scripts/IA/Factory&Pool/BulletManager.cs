using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance = default;

    [SerializeField] private Bullet _bulletPrefab = default;
    private BulletFactory _factory = default;
    private ObjectPool<ObjectToSpawn> _objectPool = default;
    [SerializeField] private int _initialCount = 5;
    [SerializeField] private bool _dynamic = true;

    public ObjectPool<ObjectToSpawn> ObjectPool { get {return _objectPool;} }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            _factory = new BulletFactory(_bulletPrefab, transform);
            _objectPool = new ObjectPool<ObjectToSpawn>(_factory.GetObj, ObjectToSpawn.TurnOff, ObjectToSpawn.TurnOn, _initialCount, _dynamic);
        }
        else Destroy(gameObject);
    }
}