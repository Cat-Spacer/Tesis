using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using InputKey;

public class KeybindManager : MonoBehaviour
{
    InputDictionary _inputDictionary;

    void Start()
    {
        _inputDictionary = new InputDictionary();

        _inputDictionary.OnStartIfNotSave();
    }
}