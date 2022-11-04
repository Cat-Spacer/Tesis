using UnityEngine;

public class ObjectToSpawn : MonoBehaviour
{
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
