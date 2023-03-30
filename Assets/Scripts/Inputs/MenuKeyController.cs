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
    InputDictionary _inputDictionary;

    private void Start()
    {
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
            _field.placeholder.gameObject.SetActive(false);
            _field.DeactivateInputField(true);
            //AssignKey(text_arg.ToString());
        }

        if (_field.text == string.Empty)
        {
            detect = false;
            _field.DeactivateInputField(true);
            _field.placeholder.gameObject.SetActive(true);
        }

        Debug.Log($"Detected key {_field.text} = {detect}");
    }

    public void CheckSpecialKeys()
    {
        //Debug.Log(_field.text);
        if (!detect) return;

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                _field.text = vKey.ToString();
                _inputDictionary.SetButtonForKey(_keyType, vKey);
                Debug.Log(vKey.ToString());
            }
        }
    }

    public void AssignKey(string keyName)
    {

    }
}
