using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuIcon : MonoBehaviour
{
    private bool canChangeScene;    
    private AsyncOperation asyncLoad;
    private void Start()
    {
        LoadSceneAsync("MainMenuCoop");
    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
        StartCoroutine(WaitForAnimationAndActivateScene());
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }
    }
    private IEnumerator WaitForAnimationAndActivateScene()
    {
        yield return new WaitUntil(() => canChangeScene);
        asyncLoad.allowSceneActivation = true;
    }
    public void ChangeScene()
    {
        canChangeScene = true;
    }
}
