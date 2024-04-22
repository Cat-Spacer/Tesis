using UnityEngine;

public class ObjectForSpawner : MonoBehaviour
{
    public ObjectForSpawner SetPosition(float x = 0, float y = 0, float z = 0)
    {
        transform.position = new Vector3(x, y, z);
        return this;
    }

    public ObjectForSpawner SetScale(float x = 1, float y = 1, float z = 1)
    {
        transform.localScale = new Vector3(x, y, z);
        return this;
    }
}
