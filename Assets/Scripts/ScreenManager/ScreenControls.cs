using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScreenControls : ScreenBase, IScreen
{
    private void Awake()
    {
        OnAwake();
    }
    public void BTN_Back()
    {
        ScreenManager.Instance.Pop(false);
    }

    public void BTN_Exit()
    {
        Free();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}