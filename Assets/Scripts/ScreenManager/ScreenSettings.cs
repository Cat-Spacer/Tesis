using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class ScreenSettings : MonoBehaviour, IScreen
{
    public static ScreenSettings instance = default;
    [SerializeField] private string _sortingLayer = "Canvas";
    private Button[] _buttons = default;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        _buttons = GetComponentsInChildren<Button>();
        SetCamera();
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }

    public void BTN_Back()
    {
        ScreenManager.Instance.Pop(false);
    }

    public void BTN_Exit()
    {
        Destroy(gameObject);
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

        return "Message Screen Deleted";
    }

    private void SetCamera()
    {
        Canvas screenCanvas = GetComponent<Canvas>();
        screenCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        screenCanvas.worldCamera = Camera.main;
        screenCanvas.sortingLayerName = _sortingLayer;

        _buttons = GetComponentsInChildren<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BTN_Back();
        }
    }
}