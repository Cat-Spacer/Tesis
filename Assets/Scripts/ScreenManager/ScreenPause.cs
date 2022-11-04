using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPause : MonoBehaviour, IScreen
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";

    private string _result;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();

        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }

    public void BTN_Settings()
    {
        _result = "Settings";

        ScreenManager.Instance.Push(_settingsScreenName);
    }

    public void BTN_Back()
    {
        _result = "Back";

        ScreenManager.Instance.Pop();
    }

    public void Activate()
    {
        foreach (var button in _buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }

    public string Free()
    {
        Destroy(gameObject);

        return _result;
    }
}
