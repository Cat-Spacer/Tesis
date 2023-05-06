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
        if (KeybindManager.Instance._inputDictionary != null)
            _inputDictionary = KeybindManager.Instance._inputDictionary;
        else
            _inputDictionary = new InputDictionary();

        if (KeybindManager.Instance.saveManager.LoadData().buttonKeys == null || KeybindManager.Instance.saveManager.LoadData().buttonValues == null)
            _inputDictionary.OnStartIfNotSave();
        else
            _inputDictionary.LoadDictionary(KeybindManager.Instance.saveManager.LoadData().buttonKeys, KeybindManager.Instance.saveManager.LoadData().buttonValues);

        if (_keyType != InputDictionary.TypeOfKeys.None)
            _keyTypeText.text = _keyType.ToString();
        else
            Debug.LogWarning("_key not assigned");

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
        }

        if (_field.text == string.Empty)
        {
            detect = false;
            _field.DeactivateInputField(true);
            _field.placeholder.gameObject.SetActive(true);
        }

        Debug.Log($"<color=aqua>Detected _key {_field.text} = {detect}</color>");
    }
    /// <summary>
    /// Check the key & replace it in the settings
    /// </summary>
    public void CheckSpecialKeys()
    {
        //Debug.Log(_field.text);
        Debug.Log($"<color=cyan>Ingrese a CheckSpecialKeys</color>");
        if (!detect) return;

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                _field.text = vKey.ToString();
                //_inputDictionary.SetButtonForKey(_keyType, vKey);
                if (!_inputDictionary.SetButtonForKey(_keyType, vKey))
                {
                    Debug.Log($"<color=yellow>Queres intercambiar las teclas?</color>");
                    KeybindManager.Instance.popUPScreen();
                }

                //else input field dejar tecla antigua

                //KeybindManager.Instance.saveManager.SaveJSON();
                Debug.Log(vKey.ToString());
            }
        }

        detect = false;
    }

    private void ExchangeKeys(KeyCode keyToChange)
    {

    }
}
