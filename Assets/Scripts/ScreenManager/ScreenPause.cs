using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPause : MonoBehaviour, IScreen
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private string _settingsScreenName = "Settings_Menu";
    [SerializeField] private string _sortingLayer = "Canvas";
    Canvas _screenCanvas;

    private string _result;

    private void Awake()
    {
        SetCamera();
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }

    public void BTN_Settings()
    {
        _result = "Settings";

        ScreenManager.Instance.PushInstance(_settingsScreenName);
    }

    public void BTN_Back()
    {
        _result = "Back";

        ScreenManager.Instance.Pop();
    }

    public void Activate()
    {
        foreach (var button in _buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }

    public string Free()
    {
        Destroy(gameObject);

        return _result;
    }

    private void SetCamera()
    {
        _screenCanvas = GetComponent<Canvas>();
        _screenCanvas.worldCamera = FindObjectOfType<Camera>();
        _screenCanvas.sortingLayerName = _sortingLayer;
        _buttons = GetComponentsInChildren<Button>();
    }
}
