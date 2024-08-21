using UnityEngine;

public class ScreenPause : ScreenBase, IScreen
{
    public static ScreenPause instance = default;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";
    [SerializeField] private string _settingsPauseScreenName = "Settings_MenuPause";
    Canvas _screenCanvas;

    private string _result;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        OnAwake();
    }

    public void BTN_Settings()
    {
        _result = "Settings";

        ScreenManager.instance.Pop();
        ScreenManager.instance.PushInstance(_settingsScreenName);
    }
    public void BTN_SettingsPause()
    {
        _result = "Settings";

        ScreenManager.instance.Pop();
        ScreenManager.instance.PushInstance(_settingsPauseScreenName);
    }

    public void BTN_Back()
    {
        _result = "Back";

        ScreenManager.instance.Pop();
    }

    public void Activate()
    {
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }
/*
    public override void SetCamera()
    {
        buttons = GetComponentsInChildren<Button>();
        if (!GetComponent<Canvas>()) return;
        _screenCanvas = GetComponent<Canvas>();
        _screenCanvas.worldCamera = Camera.main;
        _screenCanvas.sortingLayerName = _sortingLayer;
    }*/
}