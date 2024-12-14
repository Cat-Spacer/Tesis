using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private int levelTimer;
    [SerializeField] private float currentTime;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject menu;
    [SerializeField] private Image outOfTime;
    [SerializeField] private Animation anim;
    private bool _onLive;
    private bool _onLose;
    private bool _stopTimer = true;
    [SerializeField] private bool onTutorial = false;
    
    void Start()
    {
        //EventManager.Instance.Subscribe(EventType.OffLive, OnOffLive);
        //EventManager.Instance.Subscribe(EventType.OnLive, OnOnLive);
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Subscribe(EventType.StartTimer, OnStartTimer);
        EventManager.Instance.Subscribe(EventType.StopTimer, OnStopTimer);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        
        if(LiveCamera.instance != null) LiveCamera.instance.SetLevelTime(levelTimer);
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

    public void OnLive()
    {
        _onLive = true;
    }

    public void OffLive()
    {
        _onLive = false;
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
    
    private void Countdown()
    {
        if(GameManager.Instance) if(GameManager.Instance.pause) return;
        if (currentTime <= 0)
        {
            _onLose = true;
            currentTime = 0;
            SoundManager.instance.Pause(SoundsTypes.TimeBeep, gameObject);
            EventManager.Instance.Trigger(EventType.OnLoseGame);
        }
        else
        {
            currentTime -= Time.deltaTime;
            text.text = Mathf.FloorToInt(currentTime).ToString();
        }

        if (currentTime < 10)
        {
            outOfTime.gameObject.SetActive(true);
            if(!anim.isPlaying)
            {
                anim.Play();
                SoundManager.instance.Play(SoundsTypes.TimeBeep, gameObject, true);
            }
        }
    }

    public void TutorialShowTime(string time)
    {
        text.text = time;
    }

    public void TutorialDontShowTime()
    {
        text.text = "-";
    }

    private void OnDisable()
    {
        //EventManager.Instance.Unsubscribe(EventType.OffLive, OnOffLive);
        //EventManager.Instance.Unsubscribe(EventType.OnLive, OnOnLive);
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Unsubscribe(EventType.StartTimer, OnStartTimer);
        EventManager.Instance.Unsubscribe(EventType.StopTimer, OnStopTimer);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
    }
    
    private void OnDestroy()
    {
        //EventManager.Instance.Unsubscribe(EventType.OffLive, OnOffLive);
        //EventManager.Instance.Unsubscribe(EventType.OnLive, OnOnLive);
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Unsubscribe(EventType.StartTimer, OnStartTimer);
        EventManager.Instance.Unsubscribe(EventType.StopTimer, OnStopTimer);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
    }
}