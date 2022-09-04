using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private int loadingScreenIndex = 3;

    public void SceneToLoad(int scene)
    {
        AsyncLoadScenes.sceneToLoad = scene;
        ///AsyncLoadScenes.instance.ChargeAsyncScene(scene);
        SceneManager.LoadScene(loadingScreenIndex);
    }

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

    public void ExitButton() 
    {
        Application.Quit();
    }
}
