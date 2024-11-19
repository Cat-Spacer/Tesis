using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuButtons
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_onPause) Pause();
            else Resume();
        }
    }
}
