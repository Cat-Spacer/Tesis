using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSplit : MonoBehaviour
{
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }
    private void OnEnable()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnGroupCamera, OnSwitchCameraType);
        EventManager.Instance.Subscribe(EventType.OnSplitCamera, OnSplitCamera);
    }
    private void OnSplitCamera(object[] obj)
    {
        image.enabled = true;
    }

    private void OnSwitchCameraType(object[] obj)
    {
        image.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnGroupCamera, OnSwitchCameraType);
        EventManager.Instance.Unsubscribe(EventType.OnSplitCamera, OnSplitCamera);
    }
}
