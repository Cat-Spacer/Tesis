using UnityEngine;

public class SoundSpawn : ObjectToSpawn
{
    private GameObject _father = default;

    public GameObject Father { get { return _father; } }
    public SoundSpawn SetFather(GameObject father)
    {
        _father = father;
        return this;
    }
}
