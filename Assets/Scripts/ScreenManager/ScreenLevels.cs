using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScreenLevels : ScreenBase, IScreen
{
    public static ScreenLevels instance;

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
        ScreenManager.Instance.Pop(false);
    }

    public void BTN_Exit()
    {
        Free();
    }

    public void Activate()
    {
        
    }

    public void Deactivate()
    {
        
    }
}