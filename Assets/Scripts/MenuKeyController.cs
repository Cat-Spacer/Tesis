using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MenuKeyController : MonoBehaviour
{
    [SerializeField] TMP_InputField _field;
    [SerializeField] TMP_Text _keyTypeText;
    [SerializeField] TypeOfKeys _keyType;
    public enum TypeOfKeys
    {
        Jump,
        ClimbUp,
        ClimbDown,
        Left,
        Right,
        Attack,
        Interact
    }

    private void Start()
    {
        if (_keyType != null)
            _keyTypeText.text = _keyType.ToString();
        else
            Debug.LogWarning("key not assigned");
    }

    public void GetKey(TypeOfKeys key)
    {
        Event e = Event.current;
        if (e.isKey)
        {
            Debug.Log("Detected key code: " + e.keyCode);
        }
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

        if (_field.text != string.Empty)
        {
            detect = false;
            _field.placeholder.gameObject.SetActive(false);
            _field.DeactivateInputField(true);
           
        }
        if (_field.text == string.Empty)
        {
            detect = true;
            _field.DeactivateInputField(true);
            _field.placeholder.gameObject.SetActive(true);
        }

    }

    public void CheckSpecialKeys()
    {
        if (!detect) return;

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                _field.text = vKey.ToString();
                Debug.Log(vKey.ToString());
            }
        }

    }

    public void AssignKey()
    {
    }
}
