using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackWarning : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.Instance.Subscribe(EventType.OnLive, OnLive);
        EventManager.Instance.Subscribe(EventType.OffLive, OffLive);
    }

    private void OnLive(object[] obj)
    {
        animator.Play("StopHackWarning");
    }
    private void OffLive(object[] obj)
    {
        animator.Play("StartHackWarning");
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLive, OnLive);
        EventManager.Instance.Unsubscribe(EventType.OffLive, OffLive);
    }
}
