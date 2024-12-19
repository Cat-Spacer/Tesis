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
    }
    private void OnChangePeace(object[] obj)
    {
        Debug.Log("OnChangePeace");
        if (isShowing == false)
        {
            if(SoundManager.instance) SoundManager.instance.Play(SoundsTypes.CrowdSurprised);
            isShowing = true;
            animator.Play("ShowCrowd");
        }
    }

    private void OnDisable()
    {
        if(!GameManager.Instance) return;
        EventManager.Instance.Unsubscribe(EventType.OnChangePeace, OnChangePeace);
    }

    private void StopShowing()
    {
        isShowing = false;
    }
}
