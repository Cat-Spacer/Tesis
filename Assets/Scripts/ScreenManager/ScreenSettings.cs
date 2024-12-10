using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScreenSettings : ScreenBase, IScreen
{
    public static ScreenSettings instance;
    [SerializeField] private string _audioScreenName = "Audio_Menu", _controlsScreenName = "Controls_Menu", _videoScreenName = "Video_Menu";

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



    public void BTN_Audio()
    {
        //ScreenManager.instance.Pop(false);
        ScreenManager.instance.PushInstance(_audioScreenName);
    }
    
    public void BTN_Controls()
    {
        //ScreenManager.instance.Pop(false);
        ScreenManager.instance.PushInstance(_controlsScreenName);
    }
    
    public void BTN_Video()
    {
        //ScreenManager.instance.Pop(false);
        ScreenManager.instance.PushInstance(_videoScreenName);
    }

    public void BTN_Back()
    {
        ScreenManager.instance.Pop(false);
        ScreenManager.instance.lastResult = gameObject.name;
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