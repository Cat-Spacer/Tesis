using UnityEngine;

public class ObjectFactory : Factory<ObjectToSpawn>
{
    public ObjectFactory(ObjectToSpawn obj, Transform p) { prefab = obj; parent = p; }
}