using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using InputKey;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;
    public InputDictionary _inputDictionary;
    public SaveManager saveManager;
    [SerializeField] private GameObject _popUP;

    private void Awake()
    {
        Instance = this;

        if (saveManager!)
            saveManager = GetComponent<SaveManager>();
        _inputDictionary = new InputDictionary();
    }

    void Start()
    {
        saveManager.LoadJSON();

        if (saveManager.LoadData().buttonKeys == null || saveManager.LoadData().buttonValues == null || !saveManager.CheckFile())
            _inputDictionary.OnStartIfNotSave();
        else
            _inputDictionary.LoadDictionary(saveManager.LoadData().buttonKeys, saveManager.LoadData().buttonValues);
    }

    public void popUPScreen()
    {
        if (_popUP && !_popUP.activeSelf) _popUP.SetActive(true);
    }

    public void Test()
    {
        Debug.Log($"<color=teal>Testing KeybindManager components: saveManager = {saveManager} | inputDictionary = {_inputDictionary}</color>");
    }
}