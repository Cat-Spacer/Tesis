using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuIcon : MonoBehaviour
{
    private bool canChangeScene;    
    private AsyncOperation asyncLoad;
    [SerializeField] Animator animator;
    [SerializeField] Animator skipBtnAnimator;
    [SerializeField] float accelerationSpeed = 1f;
    [SerializeField] float decelerationSpeed = 1f;
    [SerializeField] private float maxAnimationSpeed;
    [SerializeField] float currentAnimationSpeed;
    [SerializeField] Button skipBtn;
    private bool isSkiping = true;
    private bool usingMouse;
    private void Start()
    {
        LoadSceneAsync("MainMenuCoop");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) || usingMouse)
        {
            currentAnimationSpeed = Mathf.Lerp(currentAnimationSpeed, maxAnimationSpeed, Time.deltaTime * accelerationSpeed);
            skipBtnAnimator.Play("SkipButtonPress");
        }
        else
        {
            currentAnimationSpeed = Mathf.Lerp(currentAnimationSpeed, 1f, Time.deltaTime * decelerationSpeed);
            skipBtnAnimator.Play("SkipButtonIdle");
        }

        animator.speed = currentAnimationSpeed;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeScene();
        }
    }

    public void Skip()
    {
        usingMouse = true;
    }

    public void StopSkip()
    {
        usingMouse = false;
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
