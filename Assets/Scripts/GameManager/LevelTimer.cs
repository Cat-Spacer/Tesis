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
    [SerializeField] Animator animator;
    bool runningOutOfTime = false;

    [SerializeField] private float[] timeWarnings;
    [SerializeField] int currentTimeWarning;
    private List<int> hackTimes;
    private int currentHackTime;
    private AudioSource _audioSource = default;
    private bool startedHackWarning;

    private void Start()
    {
        if(LiveCamera.instance != null)
        {
            LiveCamera.instance.SetLevelTime(levelTimer);
            hackTimes = LiveCamera.instance.GetHackTimes();
            currentHackTime = hackTimes.Count - 1;
        }
        text.text = levelTimer.ToString();
        currentTime = levelTimer;
        menu.SetActive(false);
        if (onTutorial) text.text = "-";
        
    }

    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Subscribe(EventType.StartTimer, OnStartTimer);
        EventManager.Instance.Subscribe(EventType.StopTimer, OnStopTimer);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnFinishGame);
    }

    private void OnFinishGame(object[] obj)
    {
        _stopTimer = true;
        menu.SetActive(false);
        _audioSource?.Stop();
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
        _audioSource?.Play();
    }
    
    private void OnPauseGame(object[] obj)
    {
        menu.SetActive(false);
        _audioSource?.Stop();
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

    private void Update()
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
            SoundManager.instance.Pause(SoundsTypes.TimeBeep);
            EventManager.Instance.Trigger(EventType.OnLoseGame, true);
        }
        else
        {
            currentTime -= Time.deltaTime;
            text.text = Mathf.FloorToInt(currentTime).ToString();
            if (currentTimeWarning <= timeWarnings.Length - 1 && currentTime <= timeWarnings[currentTimeWarning])
            {
                animator.Play("Appear");
                currentTimeWarning++;
            }

            if (currentHackTime >= 0)
            {
                if (currentTime < hackTimes[currentHackTime] + 3 && !startedHackWarning)
                {
                    startedHackWarning = true;
                    LiveCamera.instance.PlayHackingWarning();
                    EventManager.Instance.Trigger(EventType.OffLive);
                }
                if (currentTime < hackTimes[currentHackTime])
                {
                    LiveCamera.instance.EndHackingWarning();
                    LiveCamera.instance.GoOffAir();
                    if(SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.LockClose);
                    currentHackTime--;
                    startedHackWarning = false;
                }
            }
        }

        if (currentTime < 10)
        {
            if (!runningOutOfTime)
            {
                runningOutOfTime = true;
                animator.Play("OutOfTime");
                SoundManager.instance.Play(SoundsTypes.TimeBeep, null, true);
            }
        }
        if(GetComponent<AudioSource>()) _audioSource = GetComponent<AudioSource>();
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
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
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
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
}