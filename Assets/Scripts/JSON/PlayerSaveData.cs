using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InputKey;

[Serializable]
public class PlayerSaveData
{
    public InputDictionary InputDictionary;
    public float speed;
    public int lastLevel;

    public string[] myAbilities;
}