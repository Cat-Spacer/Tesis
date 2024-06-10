using UnityEngine;

public class Config : MonoBehaviour
{
    public Transform mainGame;
    [SerializeField] private string _pauseScreenName = "Pause_Menu";

    private void Start()
    {
        ScreenManager.instance.Push(new ScreenGO(mainGame));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (ScreenPause.instance)
                ScreenManager.instance.Pop();
            else
            {
                if (ScreenSettings.instance)
                    ScreenManager.instance.Pop();

                var screenPause = Instantiate(Resources.Load<ScreenPause>(_pauseScreenName));
                ScreenManager.instance.Push(screenPause);
            }
        }
    }
}