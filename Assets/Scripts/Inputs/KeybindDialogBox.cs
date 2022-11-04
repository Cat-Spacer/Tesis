using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeybindDialogBox : MonoBehaviour
{
    private InputManager inputManager;
    public GameObject keyItemPrefab;
    public GameObject keyList;

    private string buttonToRebind = null;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();

        string[] buttonNames = inputManager.GetButtonNames();

        foreach (var btn in buttonNames)
        {
            GameObject go = Instantiate(keyItemPrefab);

            Text buttonNameText = go.transform.Find("Button Name").GetComponent<Text>();
            buttonNameText.text = btn;

            Text keyNameText = go.transform.Find("Button / KeyName").GetComponent<Text>();
            keyNameText.text = inputManager.GetKeyNamesForButton(btn);

            Button keyBindButton = go.transform.Find("Button").GetComponent<Button>();
            keyBindButton.onClick.AddListener(() => { StartRebindFor(btn); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonToRebind != null)
        {
            if (Input.anyKeyDown)
            {

                KeyCode[] kcs = (KeyCode[])Enum.GetValues(typeof(KeyCode));

                foreach (KeyCode kc in kcs)
                {
                    if (Input.GetKeyDown(kc))
                    {
                        inputManager.SetButtonForKey(buttonToRebind, kc);
                        buttonToRebind = null;
                        break;
                    }
                }
            }
        }
    }

    private void StartRebindFor(string buttonName)
    {
        buttonToRebind = buttonName;
    }
}
