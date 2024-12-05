using UnityEngine;

public class ObjectToSpawn : MonoBehaviour
{
    public ObjectPool<ObjectToSpawn> objectPool = default;

    public virtual void Reset() { }

    public static void TurnOnOff(ObjectToSpawn b, bool On = true)
    {
        if (!b) return; if (!b.gameObject) return;

        b.gameObject.SetActive(On);
        if (On) b.Reset();
    }

    public virtual void AddReference(ObjectPool<ObjectToSpawn> op)
    {
        objectPool = op;
    }
}