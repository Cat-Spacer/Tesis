using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameLevelMenu : MonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private WinMenu _winMenu;
    [SerializeField] private LoseMenu _loseMenu;

    private bool _onPause = false;

    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnLoseGame);
    }
    private void OnLoseGame(object[] obj)
    {
        _loseMenu.OpenMenu();
    }
    public void WinMenu()
    {
        var points = EgoSystem.instance.GetEgoPoints();
        if(_winMenu!= null) _winMenu.OpenMenu(points.Item1, points.Item2);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnLoseGame);
    }
    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnLoseGame);
    }
}
