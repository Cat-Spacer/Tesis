using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloneable : MonoBehaviour
{
    [SerializeField] Sprite itemSprite;

    public bool _cloneObject = false;

    public void Clone(bool isClone)
    {
        _cloneObject = isClone;
    }

    public bool GetBool()
    {
        return _cloneObject;
    }

    public Sprite GetSprite()
    {
        return itemSprite;
    }
}
