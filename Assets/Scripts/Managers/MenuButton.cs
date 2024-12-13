using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private int _loadingScreenIndex = 2;

    [SerializeField] private string _settingsScreenName = "Settings_Menu",
        _controlsScreenName = "Control_Menu",
        _creditsScreenName = "Credits_Menu",
        _levelsScreenName = "Levels_Menu";

    public void BTN_Back()
    {
        if (ScreenManager.instance) ScreenManager.instance.Pop(false);
    }

    public void BTN_Controls()
    {
        if (ScreenManager.instance) ScreenManager.instance.PushInstance(_controlsScreenName);
    }

    public void BTN_Credits()
    {
        if (ScreenManager.instance) ScreenManager.instance.PushInstance(_creditsScreenName);
    }

    public void BTN_General(string screenName)
    {
        if (ScreenManager.instance) ScreenManager.instance.PushInstance(screenName);
    }

    public void BTN_Levels()
    {
        if (ScreenManager.instance) ScreenManager.instance.PushInstance(_levelsScreenName);
    }

    public void SceneToLoad(int scene)
    {
        AsyncLoadScenes.sceneToLoad = scene;
        SceneManager.LoadScene(_loadingScreenIndex);
    }

    public void BTN_Settings()
    {
        if (ScreenManager.instance) ScreenManager.instance.PushInstance(_settingsScreenName);
    }

    public void ExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}