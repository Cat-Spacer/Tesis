using UnityEngine;

public class PauseMenu : MenuButtons
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (_onFinishGame || !_onStartGame) return;
            if (!_onPause) Pause();
            else Resume();
        }
    }
}
