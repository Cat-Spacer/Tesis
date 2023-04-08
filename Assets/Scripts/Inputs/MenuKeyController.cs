using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InputKey;

public class MenuKeyController : MonoBehaviour
{
    [SerializeField] TMP_InputField _field;
    [SerializeField] TMP_Text _keyTypeText;
    [SerializeField] InputDictionary.TypeOfKeys _keyType;
    InputDictionary _inputDictionary;//usar JSON
    //SaveManager _saveManager;

    private void Start()
    {
        //_saveManager = FindObjectOfType<SaveManager>();
        _inputDictionary = new InputDictionary();
        _inputDictionary.OnStartIfNotSave();


        if (_keyType != InputDictionary.TypeOfKeys.None)
            _keyTypeText.text = _keyType.ToString();
        else
            Debug.LogWarning("key not assigned");

        _field.text = _inputDictionary.GetKeyNamesForButton(_keyType);
    }

    public void DebugKey(TMP_Text text)
    {
        Debug.Log(text.text);
    }

    public void ClearInput(TMP_Text text_arg)
    {
        text_arg.text = string.Empty;
    }

    bool detect = true;

    public void CheckInput(TMP_Text text_arg)
    {
        _field.text = _field.text.Replace(" ", "");

        if (_field.text != string.Empty || text_arg.text != string.Empty)
        {
            detect = true;
            _field.DeactivateInputField(true);
            _field.placeholder.gameObject.SetActive(false);
            //AssignKey(text_arg.ToString());
            //CheckSpecialKeys();
        }

        if (_field.text == string.Empty)
        {
            detect = false;
            _field.DeactivateInputField(true);
            _field.placeholder.gameObject.SetActive(true);
        }

        Debug.Log($"<color=aqua>Detected key {_field.text} = {detect}</color>");
    }
    /// <summary>
    /// Check the key & replace it in the settings
    /// </summary>
    public void CheckSpecialKeys()
    {
        //Debug.Log(_field.text);
        Debug.Log($"<color=cyan>Ingrese a CheckSpecialKeys</color>");/*Ingrese a CheckSpecialKeys*/
        if (!detect) return;

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                _field.text = vKey.ToString();
                _inputDictionary.SetButtonForKey(_keyType, vKey);
                KeybindManager.Instance.saveManager.SaveJSON();
                KeybindManager.Instance.Test();
                //_saveManager.SaveJSON();
                Debug.Log(vKey.ToString());
            }
        }

        detect = false;
    }

    public void AssignKey(string keyName)
    {

    }
}
