using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MushroomType
{
    Spit,
    Throw
}
[CreateAssetMenu(fileName = "New Special Mushroom", menuName = "Mushroom")]
public class SpecialMushroom_SO : ScriptableObject
{
    public MushroomType type;
    public float time;

    public virtual void Special()
    {

    }
}
