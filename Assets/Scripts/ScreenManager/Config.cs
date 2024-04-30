using UnityEngine;

public class Config : MonoBehaviour
{
    public Transform mainGame;
    [SerializeField] private string _pauseScreenName = "Pause_Menu";

    private void Start()
    {
        ScreenManager.Instance.Push(new ScreenGO(mainGame));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (FindObjectOfType<ScreenPause>())
                ScreenManager.Instance.Pop();
            else
            {
                if (FindObjectOfType<ScreenSettings>())
                    ScreenManager.Instance.Pop();

                var screenPause = Instantiate(Resources.Load<ScreenPause>(_pauseScreenName));
                ScreenManager.Instance.Push(screenPause);
            }
        }
    }
}