using UnityEditor;
using UnityEngine;

public class OwnFactory
{
    [SerializeField] private ObjectToSpawn _objPrefab = default;
    private ObjectFactory _factory = default;
    public ObjectPool<ObjectToSpawn> objectPool = default;

    public OwnFactory (ObjectToSpawn obj, Transform parent, int initialCount = 5, bool dynamic = true)
    {
        Debug.Log("Set Pool");
        _objPrefab = obj;
        _factory = new ObjectFactory(_objPrefab, parent);
        objectPool = new ObjectPool<ObjectToSpawn>(_factory.GetObj, ObjectToSpawn.TurnOff, ObjectToSpawn.TurnOn, initialCount, dynamic);
    }
}