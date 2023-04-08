using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using InputKey;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;
    [SerializeField] private InputDictionary _inputDictionary;
    [SerializeField] public SaveManager saveManager;

    private void Awake()
    {
        Instance = this; 
    }

    void Start()
    {
        if (saveManager!)
            saveManager = GetComponent<SaveManager>();
        saveManager.LoadJSON();
        _inputDictionary = new InputDictionary();
        _inputDictionary.OnStartIfNotSave();
    }

    public void Test()
    {
        Debug.Log($"<color=teal>Testing KeybindManager components: saveManager = {saveManager} | _inputDictionary = {_inputDictionary}</color>");
    }
}