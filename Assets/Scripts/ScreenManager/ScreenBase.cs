using UnityEngine;
using UnityEngine.UI;

public abstract class ScreenBase : MonoBehaviour
{
    [SerializeField] private string _sortingLayer = "Canvas";
    [SerializeField] private int _canvasLayer = 0;
    [SerializeField] public Button[] buttons = default;

    public virtual void OnAwake()
    {
        SetCamera(_canvasLayer);
        buttons = GetComponentsInChildren<Button>();
        if (buttons != null || buttons.Length <= 0) return;
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    public virtual void SetCamera(int canvasLayer = 0)
    {
        Canvas screenCanvas = GetComponent<Canvas>();
        screenCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        screenCanvas.worldCamera = Camera.main;
        screenCanvas.sortingLayerName = _sortingLayer;
        screenCanvas.sortingOrder = canvasLayer;

        buttons = GetComponentsInChildren<Button>();
    }

    public virtual string Free()
    {
        Destroy(gameObject);

        return gameObject.name;
    }
}
