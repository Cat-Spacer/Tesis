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
    private bool _startGame;
    private bool _onLive;
    private bool _onLose;
    void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Subscribe(EventType.OffLive, OnOffLive);
        EventManager.Instance.Subscribe(EventType.OnLive, OnOnLive);
        if(LiveCamera.instance != null) LiveCamera.instance.SetLevelTime(levelTimer, this);
        text.text = levelTimer.ToString();
        currentTime = levelTimer;
    }

    private void OnOnLive(object[] obj)
    {
        _onLive = true;
        menu.SetActive(true);
    }

    private void OnOffLive(object[] obj)
    {
        _onLive = false;
        menu.SetActive(false);
    }

    private void OnStartGame(object[] obj)
    {
        _startGame = true;
    }

    void Update()
    {
        if (!_startGame || !_onLive || _onLose) return;
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
    float GetLevelCurrentTime(){return currentTime;}
}
