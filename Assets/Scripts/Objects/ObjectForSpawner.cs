using UnityEngine;

public class ObjectForSpawner : Prototype
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

    public override Prototype Clone(float x = 0, float y = 0, float z = 0)
    {
        var res = Instantiate(this);

        res.SetPosition(transform.position.x + x , transform.position.y + y, transform.position.z + z)
           .SetScale();

        return res;
    }
}