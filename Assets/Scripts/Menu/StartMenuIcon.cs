using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] Book book;
    [SerializeField] AutoFlip bookAutoFlip;
    public UnityEvent OnFinishedBook;
    [SerializeField] private GameObject flipRightButtons;
    [SerializeField] private GameObject flipLeftButtons;
    private bool canFlip = false;
    public  UnityEvent FlipPage;
    private void Start()
    {
        LoadSceneAsync("MainMenuCoop");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) || usingMouse)
        {
            currentAnimationSpeed = Mathf.Lerp(currentAnimationSpeed, maxAnimationSpeed, Time.deltaTime * accelerationSpeed);
        }
        else
        {
            currentAnimationSpeed = Mathf.Lerp(currentAnimationSpeed, 1f, Time.deltaTime * decelerationSpeed);
        }

        animator.speed = currentAnimationSpeed;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeScene();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && canFlip)
        {
            if(FlipPage != null && !bookAutoFlip.isFlipping)
            {
                bookAutoFlip.FlipRightPage();
                FlipPage.Invoke();
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && canFlip)
        {
            if(FlipPage != null)
            {
                if(book.currentPage > 0 && !bookAutoFlip.isFlipping)
                {
                    bookAutoFlip.FlipLeftPage();
                    FlipPage.Invoke();
                }
            }
        }
    }

    public void FlipRightPage()
    {
        if(FlipPage != null && !bookAutoFlip.isFlipping)
        {
            bookAutoFlip.FlipRightPage();
            FlipPage.Invoke();
        }
        
    }
    public void FlipLeftPage()
    {
        if(FlipPage != null)
        {
            if(book.currentPage > 0 && !bookAutoFlip.isFlipping)
            {
                bookAutoFlip.FlipLeftPage();
                FlipPage.Invoke();
            }
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

    public void CanFlipBook()
    {
        canFlip = true;
    }
    public void CheckCurrentPage()
    {
        var currentPage = book.currentPage;
        if (currentPage > book.bookPages.Length - 3)
        {
            flipRightButtons.SetActive(false);
            flipLeftButtons.SetActive(false);
            animator.enabled = true;
            canFlip = false;
            animator.Play("ChangeScene");
        }
    }
    void StopAnimator()
    {
        animator.enabled = false;
    }
}
