using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float levelTimer;
    [SerializeField] private float currentTime;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject menu;
    bool _startGame;
    private bool _onLive;
    void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Subscribe(EventType.OffLive, OnOffLive);
        EventManager.Instance.Subscribe(EventType.OnLive, OnOnLive);
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
        if (!_startGame || !_onLive) return;
        Countdown();
    }

    void Countdown()
    {
        if (currentTime <= 0)
        {
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
