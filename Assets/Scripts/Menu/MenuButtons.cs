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
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnLoseGame);
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
    protected virtual void OnLoseGame(object[] obj)
    {
        _onFinishGame = true;
        if (GameManager.Instance) StartCoroutine(GameManager.Instance.DisableByBehaviour());
    }
    public virtual void OpenMenu()
    {
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
        if (GameManager.Instance) StartCoroutine(GameManager.Instance.DisableByBehaviour());
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
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnLoseGame);
    }
}