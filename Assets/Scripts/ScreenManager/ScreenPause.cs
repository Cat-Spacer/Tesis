using UnityEngine;

public class ScreenPause : ScreenBase, IScreen
{
    public static ScreenPause instance = default;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";
    [SerializeField] private string _settingsPauseScreenName = "Settings_MenuPause";
    Canvas _screenCanvas;

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

        ScreenManager.instance.Pop();
        ScreenManager.instance.PushInstance(_settingsScreenName);
    }
    public void BTN_SettingsPause()
    {
        ScreenManager.instance.Pop();
        ScreenManager.instance.PushInstance(_settingsPauseScreenName);
    }

    public void BTN_Back()
    {
        ScreenManager.instance.Pop();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}