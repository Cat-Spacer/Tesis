﻿using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScreenAudio : ScreenBase, IScreen
{
    public static ScreenAudio instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        OnAwake();
    }

    public void BTN_Back()
    {
        ScreenManager.instance.Pop(false);
    }

    public void BTN_Exit()
    {
        Free();
    }

    public void Activate()
    {
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }
}