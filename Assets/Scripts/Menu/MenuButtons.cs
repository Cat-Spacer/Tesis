using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;
    protected bool _onPause, _onFinishGame, _onStartGame;
    private IScreen _pauseScreen = null;

    private void Start()
    {
        _menu.SetActive(false);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnFinishGame);
    }

    private void OnStartGame(object[] obj)
    {
        _onStartGame = true;
        if (GameManager.Instance) GameManager.Instance.EnableByBehaviour();
    }

    protected virtual void OnFinishGame(object[] obj)
    {
        _onFinishGame = true;
        if (GameManager.Instance) StartCoroutine(GameManager.Instance.DisableByBehaviour());
    }

    public virtual void OpenMenu()
    {
        //Time.timeScale = 0;

        if (GameManager.Instance) StartCoroutine(GameManager.Instance.DisableByBehaviour());
        _menu.SetActive(true);
    }

    public virtual void NextLevel()
    {
        AsyncLoadScenes.sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene("LoadScene");
    }

    public virtual void Resume()
    {
        _onPause = false;
        if (GameManager.Instance) GameManager.Instance.EnableByBehaviour();

        EventManager.Instance.Trigger(EventType.OnResumeGame);
        EventManager.Instance.Trigger(EventType.ReturnGameplay);
        SoundManager.instance.Play(SoundsTypes.Click);
        _menu.SetActive(false);
    }

    public virtual void Pause()
    {
        EventManager.Instance.Trigger(EventType.OnPauseGame);
        EventManager.Instance.Trigger(EventType.ShowTv);
        SoundManager.instance.Play(SoundsTypes.Click);
        _onPause = true;
        if (GameManager.Instance) StartCoroutine(GameManager.Instance.DisableByBehaviour());
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        _menu.SetActive(true);
    }

    public virtual void ReturnToMenu()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public virtual void RestartLevel()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public virtual void LevelSelector()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
}