using UnityEngine;

public class OwnFactory
{
    [SerializeField] private ObjectToSpawn _objPrefab = default;
    private ObjectFactory _factory = default;
    private ObjectPool<ObjectToSpawn> _objectPool = default;

    public ObjectPool<ObjectToSpawn> ObjectPool { get { return _objectPool; } }

    public OwnFactory (ObjectToSpawn obj, Transform parent, int initialCount = 5, bool dynamic = true)
    {
        Debug.Log("Set Pool");
        _objPrefab = obj;
        _factory = new ObjectFactory(_objPrefab, parent);
        _objectPool = new ObjectPool<ObjectToSpawn>(_factory.GetObj, ObjectToSpawn.TurnOff, ObjectToSpawn.TurnOn, initialCount, dynamic);
    }
}