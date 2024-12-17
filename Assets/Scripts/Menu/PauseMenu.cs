using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuButtons
{
    [SerializeField] private Button pauseButton; 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (_onFinishGame || !_onStartGame) return;
            if (!_onPause && !tv.OnFinishTransition())
            {
                pauseButton.enabled = false;
                Pause();
            }
            else if(!tv.OnFinishTransition())
            {
                pauseButton.enabled = false;
                Resume();
            }
        }
    }

    protected override IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(ButtonEnableDelay());
        _menu.SetActive(true);
        foreach (var button in tv.tvButtons) button.gameObject.SetActive(true);
    }
    private IEnumerator ButtonEnableDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        pauseButton.enabled = true;
    }
}
