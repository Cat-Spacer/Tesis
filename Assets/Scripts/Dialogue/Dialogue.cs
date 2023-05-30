using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Dialogue
{
    public string charName = string.Empty;

    [TextArea(3, 10)]
    public List<string> sentences = new List<string>();
}