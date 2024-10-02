using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    WASD,
    Arrow
}
[CreateAssetMenu(fileName = "Inputs", menuName = "Input")]
public class SO_Inputs : ScriptableObject
{
    public Type inputType;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode attack;
    public KeyCode interact;
    public KeyCode special;
}
