using System.Collections.Generic;
using UnityEngine;

public class SoundSpawn : ObjectToSpawn
{
    private GameObject _father = default;
    public List<Sound> sounds = new ();

    public GameObject Father { get { return _father; } }
    public SoundSpawn SetFather(GameObject father)
    {
        _father = father;
        gameObject.transform.parent = _father.transform;
        return this;
    }
}
