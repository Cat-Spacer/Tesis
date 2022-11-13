using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using InputKey;

    public class KeybindManager : MonoBehaviour
    {

    InputDictionary inputDictionary;
    void Start()
    {
        inputDictionary = new InputDictionary();

        inputDictionary.OnStartIfNotSave();
    }
 
}

   
