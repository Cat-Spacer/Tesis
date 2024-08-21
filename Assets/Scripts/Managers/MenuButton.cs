using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public static MenuButton instance = default;
    [SerializeField] private int loadingScreenIndex = 0;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";
    [SerializeField] private string _controlsScreenName = "Control_Menu";
    private IScreen _activeScreen;

    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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

    public void BTN_Controls()
    {
        ScreenManager.instance.PushInstance(_controlsScreenName);
    }
    
    public void BTN_General(string screenName)
    {
        ScreenManager.instance.PushInstance(screenName);
    }
    public void BTN_Back()
    {
        ScreenManager.instance.Pop(false);
    }



    public void ExitButton() 
    {
        Application.Quit();
    }
}