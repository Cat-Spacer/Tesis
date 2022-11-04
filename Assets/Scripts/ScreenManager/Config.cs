using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public Transform mainGame;
    [SerializeField] private string _pauseScreenName = "Pause_Menu";
    private bool _isPaused = false;

    private void Start()
    {
        ScreenManager.Instance.Push(new ScreenGO(mainGame));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ScreenManager.Instance.Pop();
                _isPaused = false;
            }
            else
            {
                _isPaused = true;
                var screenPause = Instantiate(Resources.Load<ScreenPause>(_pauseScreenName));
                ScreenManager.Instance.Push(screenPause);
            }
        }
    }
}
