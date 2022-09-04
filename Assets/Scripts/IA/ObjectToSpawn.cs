using UnityEngine;

public class ObjectToSpawn : MonoBehaviour
{
    private void Awake()
    {
        if (ObjectFactory.Instance != null)
            ObjectFactory.Instance.ReturnObject(this);
    }

    private void Reset()
    {

    }

    public static void TurnOn(ObjectToSpawn b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(ObjectToSpawn b)
    {
        b.gameObject.SetActive(false);
    }
}
