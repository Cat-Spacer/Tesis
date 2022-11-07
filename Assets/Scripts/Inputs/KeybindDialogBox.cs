using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class KeybindDialogBox : MonoBehaviour
{
    public bool autoGenerateButtons = false;
    public GameObject keyItemPrefab;
    public GameObject keyList;
    private InputManager _inputManager;
    private Dictionary<string, TextMeshProUGUI> _buttonToLabel;

    private string _buttonToRebind = null;

    [Header("Set manual Buttons")]
    [SerializeField] private TextMeshProUGUI[] _keyName;

    void Start()
    {
        _buttonToLabel = new Dictionary<string, TextMeshProUGUI>();
#if autoGenerateButtons
        _inputManager = FindObjectOfType<InputManager>();
        string[] buttonNames = _inputManager.GetButtonNames();
        Dictionary<string, KeyCode[]> keyValuePairs = _inputManager.GetButtonKeys();

        foreach (var btn in buttonNames)
        {
            GameObject go = Instantiate(keyItemPrefab);
            go.transform.SetParent(keyList.transform);
            go.transform.localScale = Vector3.one;
            foreach (var keyCode in _buttonToLabel.Values)
            {
                TextMeshProUGUI buttonNameText = go.transform.Find("Button Name (TMP)").GetComponent<TextMeshProUGUI>();
                buttonNameText.text = btn;

                TextMeshProUGUI keyNameText = go.transform.Find("Button/Key Name (TMP)").GetComponent<TextMeshProUGUI>();
                keyNameText.text = _inputManager.GetKeyNamesForButton(btn);
                _buttonToLabel[btn] = keyNameText;

                Button keyBindButton = go.transform.Find("Button").GetComponent<Button>();
                keyBindButton.onClick.AddListener(() => { StartRebindFor(btn); });
            }
            // Hacer boton alternativo
        }
#else
        foreach (var btnTxt in _keyName)
        {
            _buttonToLabel[btnTxt.text] = btnTxt;
        }
#endif


    }
    void Update()
    {
        if (_buttonToRebind != null)
        {
            if (Input.anyKeyDown)
            {

                KeyCode[] kcs = (KeyCode[])Enum.GetValues(typeof(KeyCode));

                foreach (KeyCode kc in kcs)
                {
                    if (Input.GetKeyDown(kc))
                    {
                        if (_inputManager.SetButtonForKey(_buttonToRebind, kc))
                            _buttonToLabel[_buttonToRebind].text = kc.ToString();
                        _buttonToRebind = null;
                        break;
                    }
                }
            }
        }
    }

    private void StartRebindFor(string buttonName)
    {
        _buttonToRebind = buttonName;
    }
}
