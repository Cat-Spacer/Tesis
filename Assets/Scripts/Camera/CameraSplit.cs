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
}
