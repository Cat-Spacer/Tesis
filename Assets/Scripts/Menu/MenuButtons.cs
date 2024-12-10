using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;
    protected bool _onPause, _onFinishGame, _onStartGame;
    void Start()
    {
        _menu.SetActive(false);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnFinishGame);
    }

    private void OnStartGame(object[] obj)
    {
        _onStartGame = true;
    }

    private void OnFinishGame(object[] obj)
    {
        _onFinishGame = true;
    }

    public virtual void OpenMenu()
    {
        Time.timeScale = 0;
        //SoundManager.instance.Play(SoundsTypes.Click);
        _menu.SetActive(true);
    }
    public virtual void NextLevel()
    {
        //SoundManager.instance.Play(SoundsTypes.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public virtual void Resume()
    {
        Time.timeScale = 1;
        _onPause = false;
        EventManager.Instance.Trigger(EventType.OnResumeGame);
        SoundManager.instance.Play(SoundsTypes.Click);
        _menu.SetActive(false);
    }
    public virtual void Pause()
    {
        EventManager.Instance.Trigger(EventType.OnPauseGame);
        Time.timeScale = 0;
        SoundManager.instance.Play(SoundsTypes.Click, null);
        _onPause = true;
        _menu.SetActive(true);
    }
    public virtual void ReturnToMenu()
    {
        Time.timeScale = 1;
        //SoundManager.instance.Play(SoundsTypes.Click);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public virtual void RestartLevel()
    {
        Time.timeScale = 1;
        //SoundManager.instance.Play(SoundsTypes.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public virtual void LevelSelector()
    {
        Time.timeScale = 1;
        //SoundManager.instance.Play(SoundsTypes.Click);
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
}
