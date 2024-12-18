using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuButtons
{
    private Action enableButtonAction = delegate { };
    private float currentTime;
    private float timeCount = 1;
    private bool pausing;
    private void Update()
    {
        enableButtonAction();
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (_onFinishGame || !_onStartGame || tv.OnFinishTransition() || pausing) return;
            pausing = true;
            if (!_onPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public override void Resume()
    {
        EventManager.Instance.Trigger(EventType.ReturnGameplay);
        enableButtonAction = CountdownResume;
        _onPause = false;
        _menu.SetActive(false);
    }

    void CountdownPause()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeCount)
        {
            currentTime = 0;
            pausing = false;
            enableButtonAction = delegate { };
        }
    }
    void CountdownResume()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeCount)
        {
            currentTime = 0;
            pausing = false;
            enableButtonAction = delegate { };
        }
    }
    protected override IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        enableButtonAction = CountdownPause;
        _menu.SetActive(true);
        foreach (var button in tv.tvButtons) button.gameObject.SetActive(true);
    }
}
