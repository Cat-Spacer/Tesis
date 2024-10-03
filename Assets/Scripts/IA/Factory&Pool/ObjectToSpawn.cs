using UnityEngine;

public class ObjectToSpawn : MonoBehaviour
{
    public ObjectPool<ObjectToSpawn> objectPool = default;

    public virtual void Reset() { }

    public static void TurnOn(ObjectToSpawn b)
    {
        b.gameObject.SetActive(true);
        b.Reset();
    }

    public static void TurnOff(ObjectToSpawn b)
    {
        b.gameObject.SetActive(false);
    }

    public virtual void AddReference(ObjectPool<ObjectToSpawn> op)
    {
        objectPool = op;
    }
}