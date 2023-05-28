using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string charName = string.Empty;

    [TextArea(3, 10)]
    public string[] sentences = null;
}
