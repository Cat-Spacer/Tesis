using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance = default;

    public Bullet bulletPrefab = default;
    private BulletFactory factory = default;
    public ObjectPool<Bullet> objectPool = default;

    private void Awake()
    {
        if (instance!)
        {
            instance = this;
            factory = new BulletFactory(bulletPrefab);
            objectPool = new ObjectPool<Bullet>(factory.GetObj, ObjectToSpawn.TurnOff, ObjectToSpawn.TurnOn, 4);
        }
        else Destroy(gameObject);
    }
}