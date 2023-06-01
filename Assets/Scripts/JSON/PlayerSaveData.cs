using System.Collections.Generic;
using UnityEngine;
using System;
using InputKey;

[Serializable]
public class PlayerSaveData
{
    //---------- Player Inputs ----------//
    public List<InputDictionary.TypeOfKeys> buttonKeys;
    public List<KeyCode> buttonValues;

    //---------- Player Stats ----------//
    public float speed;
    public int lastLevel;
    public string[] myAbilities,mySpritesName;

    //---------- Scene Dialogues ----------//
    public List<string[]> dialogues;
    public string[] charNames, imageNames;
}