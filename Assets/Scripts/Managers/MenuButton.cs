using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private int loadingScreenIndex = 0;
    [SerializeField] private string _settingsScreenName = "Settings_Menu", _controlsScreenName = "Control_Menu", _creditsScreenName = "Credits_Menu", _levelsScreenName = "Levels_Menu";

    public void BTN_Back()
    {
        ScreenManager.instance.Pop(false);
    }

    public void BTN_Controls()
    {
        ScreenManager.instance.PushInstance(_controlsScreenName);
    }

    public void BTN_Credits()
    {
        ScreenManager.instance.PushInstance(_creditsScreenName);
    }

    public void BTN_General(string screenName)
    {
        ScreenManager.instance.PushInstance(screenName);
    }

    public void BTN_Levels()
    {
        ScreenManager.instance.PushInstance(_levelsScreenName);
    }

    public void SceneToLoad(int scene)
    {
        AsyncLoadScenes.sceneToLoad = scene;
        SceneManager.LoadScene(loadingScreenIndex);
    }

    public void BTN_Settings()
    {
        ScreenManager.instance.PushInstance(_settingsScreenName);
    }

    public void ExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}