using UnityEngine;

public class BulletManager : MonoBehaviour//Si sigue fallando que cada uno tenga su object pool
{
    public static BulletManager Instance;

    public Bullet bulletPrefab;
    private BulletFactory factory;
    public ObjectPool<Bullet> objectPool;

    private void Awake()
    {
        if (Instance!)
        {
            Instance = this;
            factory = new BulletFactory(bulletPrefab);
            objectPool = new ObjectPool<Bullet>(factory.GetObj, ObjectToSpawn.TurnOff, ObjectToSpawn.TurnOn, 4);
        }
        else Destroy(gameObject);
    }
}