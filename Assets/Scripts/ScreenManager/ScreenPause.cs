using UnityEngine;

public class ScreenPause : ScreenBase, IScreen
{
    public static ScreenPause instance = default;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";
    [SerializeField] private string _settingsPauseScreenName = "Settings_MenuPause";
    private Canvas _screenCanvas;

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

        ScreenManager.Instance.Pop();
        ScreenManager.Instance.PushInstance(_settingsScreenName);
    }
    public void BTN_SettingsPause()
    {
        ScreenManager.Instance.Pop();
        ScreenManager.Instance.PushInstance(_settingsPauseScreenName);
    }

    public void BTN_Back()
    {
        ScreenManager.Instance.Pop();
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