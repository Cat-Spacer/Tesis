using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;
    protected bool _onPause;
    protected bool onFinishGame;
    protected bool onStartGame;
    void Start()
    {
        _menu.SetActive(false);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
    }

    private void OnStartGame(object[] obj)
    {
        onStartGame = true;
    }

    private void OnFinishGame(object[] obj)
    {
        onFinishGame = true;
    }

    public virtual void OpenMenu()
    {
        Time.timeScale = 0;
        SoundManager.instance.Play(SoundsTypes.Button);
        _menu.SetActive(true);
    }
    public virtual void NextLevel()
    {
        SoundManager.instance.Play(SoundsTypes.Button);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public virtual void Resume()                                 
    {                
        EventManager.Instance.Trigger(EventType.OnResumeGame);
        Time.timeScale = 1;   
        SoundManager.instance.Play(SoundsTypes.Button);
        _onPause = false;                         
        _menu.SetActive(false);              
    }
    public virtual void Pause()
    {
        EventManager.Instance.Trigger(EventType.OnPauseGame);
        Time.timeScale = 0;
        SoundManager.instance.Play(SoundsTypes.Button);
        _onPause = true;
        _menu.SetActive(true);
    }
    public virtual void ReturnToMenu()
    {
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundsTypes.Button);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public virtual void RestartLevel()
    {
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundsTypes.Button);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public virtual void LevelSelector()
    {
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundsTypes.Button);
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }
}
