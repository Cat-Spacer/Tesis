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
        ScreenManager.Instance.PushInstance(_audioScreenName);
    }
    
    public void BTN_Controls()
    {
        ScreenManager.Instance.PushInstance(_controlsScreenName);
    }
    
    public void BTN_Video()
    {
        ScreenManager.Instance.PushInstance(_videoScreenName);
    }

    public void BTN_Back()
    {
        ScreenManager.Instance.Pop(false);
        ScreenManager.Instance.lastResult = gameObject.name;
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