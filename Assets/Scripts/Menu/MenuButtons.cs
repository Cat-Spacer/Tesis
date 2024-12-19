using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] protected UICamera tv;
    [SerializeField] protected GameObject _menu;
    protected bool _onPause, _onFinishGame, _onStartGame;
    private IScreen _pauseScreen = null;

    protected virtual void Start()
    {
        _menu.SetActive(false);
    }
    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnLoseGame);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
    }

    private void OnFinishGame(object[] obj)
    {
        _onFinishGame = true;
    }

    private void OnStartGame(object[] obj)
    {
        _onStartGame = true;
    }
    
    protected virtual void OnLoseGame(object[] obj)
    {
        _onFinishGame = true;
    }
    public virtual void OpenMenu()
    {
        _menu.SetActive(true);
    }

    public virtual void NextLevel()
    {
        AsyncLoadScenes.sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene("LoadScene");
    }

    public virtual void Resume()
    {
        EventManager.Instance.Trigger(EventType.ReturnGameplay);
        _onPause = false;
        _menu.SetActive(false);
    }

    public virtual void Pause()
    {
        EventManager.Instance.Trigger(EventType.OnPauseGame);
        EventManager.Instance.Trigger(EventType.ShowTv);
        SoundManager.instance.Play(SoundsTypes.Click);
        _onPause = true;
        StartCoroutine(Delay());
    }

    protected virtual IEnumerator Delay() 
    {
        yield return new WaitForSecondsRealtime(0.1f);
        _menu.SetActive(true);
        foreach (var button in tv.tvButtons) button.gameObject.SetActive(true);
    }

    public virtual void ReturnToMenu()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public virtual void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public virtual void LevelSelector()
    {
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnLoseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
    }
}