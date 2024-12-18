using UnityEngine;

public abstract class ScreenBase : MonoBehaviour
{
    [SerializeField] private string _sortingLayer = "Canvas";
    [SerializeField] private int _canvasLayer = 0;
    [SerializeField] private RenderMode _render = default;

    public virtual void OnAwake()
    {
        SetCamera(_canvasLayer);
    }
    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnResumeGame, FreeEvent);
    }
    public virtual void SetCamera(int canvasLayer = 0)
    {
        Canvas screenCanvas = GetComponent<Canvas>();
        screenCanvas.renderMode = _render;
        screenCanvas.worldCamera = Camera.main;
        screenCanvas.sortingLayerName = _sortingLayer;
        screenCanvas.sortingOrder = canvasLayer;
    }

    public virtual string Free()
    {
        Destroy(gameObject);

        return gameObject.name;
    }

    private void FreeEvent(object[] obj)
    {
        Free();
    }

    private void OnDestroy()
    {
        if (EventManager.Instance) EventManager.Instance.Unsubscribe(EventType.OnResumeGame, FreeEvent);
    }
}