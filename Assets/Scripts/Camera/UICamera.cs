using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UICamera : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Image screen;
    [SerializeField] private Image tv;
    [SerializeField] private CinemachineVirtualCamera normalCam;
    [SerializeField] private CinemachineVirtualCamera zoomOutCam;
    [SerializeField] Camera uiCam;
    [SerializeField] private Camera gameplayCam;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject buttons;

    [SerializeField] GameObject cameraShader;
    
    [SerializeField] public Button[] tvButtons;
    [SerializeField] public Button resumeBtn;
    [SerializeField] public Button nextLevelBtn;
    [SerializeField] public Button restartBtn;
    [SerializeField] public Button levelsBtn;
    [SerializeField] public Button optionsBtn;
    [SerializeField] public Button menuBtn;
    private int width, height;
    private bool onTransition = true;
    private void Start()
    {
        //uiCam.gameObject.SetActive(true);
        animator = GetComponent<Animator>();
        //gameplayCam.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.ShowTv, OnShowTv);
        EventManager.Instance.Subscribe(EventType.ReturnGameplay, OnResumeGameplay);
    }
    public void StartGameplay()
    {
        animator.Play("StartGameplay");
    }
    private void OnShowTv(object[] obj)
    {
        StartCoroutine(Screenshot());
    }
    private void OnResumeGameplay(object[] obj)
    {
        normalCam.Priority = 1;
        zoomOutCam.Priority = 0;
        onTransition = true;
        foreach (var button in tvButtons) button.enabled = false;
        StartCoroutine(ResumeGameplay());
    }

    IEnumerator Screenshot()
    {
        yield return new WaitForEndOfFrame();
        width = Screen.width;
        height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
            
        screen.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
        cameraShader.SetActive(true);
        gameplayCam.gameObject.SetActive(false);
        tv.gameObject.SetActive(true);
        screen.gameObject.SetActive(true);
        uiCam.gameObject.SetActive(true);
        buttons.SetActive(true);
        normalCam.Priority = 2;
        zoomOutCam.Priority = 1;
        onTransition = true;
        StartCoroutine(ZoomOut()); 
    }

    IEnumerator ZoomOut()
    {
        yield return new WaitForSecondsRealtime(.25f);
        normalCam.Priority = 1;
        zoomOutCam.Priority = 2;
        StartCoroutine(FinishZoomOut());
    }

    IEnumerator FinishZoomOut()
    {
        yield return new WaitForSeconds(1);
        foreach (var button in tvButtons) button.enabled = true;
        onTransition = false;
    }
    IEnumerator ResumeGameplay()
    {
        yield return new WaitForSecondsRealtime(1f);
        cameraShader.SetActive(false);
        tv.gameObject.SetActive(false);
        screen.gameObject.SetActive(false);
        screen.gameObject.SetActive(false);
        buttons.SetActive(false);
        uiCam.gameObject.SetActive(false);
        gameplayCam.gameObject.SetActive(true);
        EventManager.Instance.Trigger(EventType.OnResumeGame);
        SoundManager.instance.Play(SoundsTypes.Click);
        if (GameManager.Instance) GameManager.Instance.EnableByBehaviour();
        onTransition = false;
    }

    public void CloseAllButtons()
    {
        foreach (var button in tvButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void OpenAllButtons()
    {
        foreach (var button in tvButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public bool OnFinishTransition()
    {
        return onTransition;
    }
    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.ShowTv, OnShowTv);
        EventManager.Instance.Unsubscribe(EventType.ReturnGameplay, OnResumeGameplay);
    }
}
