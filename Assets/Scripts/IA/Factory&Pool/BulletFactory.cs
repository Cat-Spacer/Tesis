using UnityEngine;

public class BulletFactory : Factory<Bullet>
{
    public BulletFactory(Bullet b, Transform p) { prefab = b; parent = p; }
}