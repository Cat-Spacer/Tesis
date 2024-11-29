using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private int levelTimer;
    [SerializeField] private float currentTime;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject menu;
    private bool _onLive;
    private bool _onLose;
    private bool _stopTimer = true;
    [SerializeField] private bool onTutorial = false;
    void Start()
    {
        EventManager.Instance.Subscribe(EventType.OffLive, OnOffLive);
        EventManager.Instance.Subscribe(EventType.OnLive, OnOnLive);
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Subscribe(EventType.StartTimer, OnStartTimer);
        EventManager.Instance.Subscribe(EventType.StopTimer, OnStopTimer);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        
        if(LiveCamera.instance != null) LiveCamera.instance.SetLevelTime(levelTimer, this);
        text.text = levelTimer.ToString();
        currentTime = levelTimer;
        menu.SetActive(true);
        if (onTutorial) text.text = "-";
    }

    private void OnFinishGame(object[] obj)
    {
        _stopTimer = true;
        menu.SetActive(false);
    }

    private void OnStopTimer(object[] obj)
    {
        _stopTimer = true;
    }
    private void OnStartTimer(object[] obj)
    {
        _stopTimer = false;
        menu.SetActive(true);
    }
    private void OnResumeGame(object[] obj)
    {
        menu.SetActive(true);
    }
    private void OnPauseGame(object[] obj)
    {
        menu.SetActive(false);
    }

    private void OnOnLive(object[] obj)
    {
        _onLive = true;
    }

    private void OnOffLive(object[] obj)
    {
        _onLive = false;
    }
    void Update()
    {
        if (_stopTimer || !_onLive || _onLose || onTutorial) return;
        Countdown();
    }
    void Countdown()
    {
        if (currentTime <= 0)
        {
            _onLose = true;
            currentTime = 0;
            EventManager.Instance.Trigger(EventType.OnLoseGame);
        }
        else
        {
            currentTime -= Time.deltaTime;
            text.text = Mathf.FloorToInt(currentTime).ToString();
        }
    }
}
