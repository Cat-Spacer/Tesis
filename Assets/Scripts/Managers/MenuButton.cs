using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Networking.UnityWebRequest;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private int loadingScreenIndex = 0;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";

    public void SceneToLoad(int scene)
    {
        AsyncLoadScenes.sceneToLoad = scene;
        ///AsyncLoadScenes.instance.ChargeAsyncScene(scene);
        SceneManager.LoadScene(loadingScreenIndex);
    }
    public void BTN_Settings()
    {
        ScreenManager.Instance.PushInstance(_settingsScreenName);
    }
    #region Async Scene Load
    /*
    public void ChargeAsyncScene(int scene)
    {
        var async = SceneManager.LoadSceneAsync(scene);

        StartCoroutine(ChargeCoroutine(async));
    }

    private IEnumerator ChargeCoroutine(AsyncOperation async)
    {
        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;
    }*/
    #endregion
    public void ExitButton() 
    {
        Application.Quit();
    }
}
