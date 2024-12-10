using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScreenCredits : ScreenBase, IScreen
{
    private void Awake()
    {
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
        if(buttons == null || buttons.Length <= 0) return;
        foreach (var button in buttons)
        {
            if (button != null)
                button.interactable = true;
        }
    }

    public void Deactivate()
    {
        if (buttons == null || buttons.Length <= 0) return;
        foreach (var button in buttons)
        {
            if (button != null)
                button.interactable = false;
        }
    }
}
