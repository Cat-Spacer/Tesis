using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    Animator animator;
    private bool isShowing;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if(!GameManager.Instance) return;
        EventManager.Instance.Subscribe(EventType.OnChangePeace, OnChangePeace);
        EventManager.Instance.Subscribe(EventType.OnGetHappy, OnGetHappy);
    }

    private void OnGetHappy(object[] obj)
    {
        if (isShowing == false)
        {
            if(SoundManager.instance) SoundManager.instance.Play(SoundsTypes.CrowdHappy);
            isShowing = true;
            animator.Play("ShowHappyCrowd");
        }
    }

    private void OnChangePeace(object[] obj)
    {
        if (isShowing == false)
        {
            if(SoundManager.instance) SoundManager.instance.Play(SoundsTypes.CrowdSurprised);
            isShowing = true;
            animator.Play("ShowSadCrowd");
        }
    }

    private void OnDisable()
    {
        if(!GameManager.Instance) return;
        EventManager.Instance.Unsubscribe(EventType.OnChangePeace, OnChangePeace);
        EventManager.Instance.Unsubscribe(EventType.OnGetHappy, OnGetHappy);
    }

    private void StopShowing()
    {
        isShowing = false;
    }
}
