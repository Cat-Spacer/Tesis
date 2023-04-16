using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InputKey;

[Serializable]
public class PlayerSaveData
{
    public List<InputDictionary.TypeOfKeys> buttonKeys;
    public List<KeyCode> buttonValues;
    public float speed;
    public int lastLevel;

    public string[] myAbilities;
}