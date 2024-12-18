using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackWarning : MonoBehaviour
{
    Animator animator;
    [SerializeField] private bool firstTime = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnLive, OnLive);
        EventManager.Instance.Subscribe(EventType.OffLive, OffLive);
    }

    private void OnLive(object[] obj)
    {
        if (firstTime)
        {
            firstTime = false;
            return;
        }
        Debug.Log("StartHack");
        animator.Play("StopHackWarning");
    }
    private void OffLive(object[] obj)
    {
        Debug.Log("StopHack");
        animator.Play("StartHackWarning");
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLive, OnLive);
        EventManager.Instance.Unsubscribe(EventType.OffLive, OffLive);
    }
    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLive, OnLive);
        EventManager.Instance.Unsubscribe(EventType.OffLive, OffLive);
    }
}
