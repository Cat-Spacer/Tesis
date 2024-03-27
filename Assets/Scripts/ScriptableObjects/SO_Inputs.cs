using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Coop,
    Multiplier
}
[CreateAssetMenu(fileName = "Inputs", menuName = "Input")]
public class SO_Inputs : ScriptableObject
{
    public HamsterInput coop;
    public HamsterInputMulti multi;
}
