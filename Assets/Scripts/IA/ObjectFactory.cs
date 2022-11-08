using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    public static ObjectFactory Instance
    {
        get
        {
            return _instance;
        }
    }
    static ObjectFactory _instance;

    public ObjectToSpawn objectPrefab;
    public int objectStock = 10;
    public bool isDynamic = true;

    public ObjectPool<ObjectToSpawn> pool;

    void Start()
    {
        _instance = this;
        pool = new ObjectPool<ObjectToSpawn>(ObjectCreator, ObjectToSpawn.TurnOn, ObjectToSpawn.TurnOff, objectStock, isDynamic);
    }

    public ObjectToSpawn ObjectCreator()
    {
        return Instantiate(objectPrefab);
    }

    public void ReturnObject(ObjectToSpawn b)
    {
        if (pool != null && b != null)
        {
            pool.ReturnObject(b);
        }
    }
}