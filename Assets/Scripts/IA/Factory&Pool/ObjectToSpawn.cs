using UnityEngine;
using UnityEngine.Rendering;

public class ObjectToSpawn : MonoBehaviour
{
    public ObjectPool<ObjectToSpawn> objectPool = default;

    private void Reset(){}

    public static void TurnOn(ObjectToSpawn b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(ObjectToSpawn b)
    {
        b.gameObject.SetActive(false);
    }

    public void AddReference(ObjectPool<ObjectToSpawn> op)
    {
        objectPool = op;
    }
}