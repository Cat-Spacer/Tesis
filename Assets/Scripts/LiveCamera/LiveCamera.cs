using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LiveCamera : MonoBehaviour
{
    public static LiveCamera instance;
    private int _levelTime;
    [SerializeField] private int _hackTime;
    [SerializeField] private List<int> _hackTimes = new List<int>();
    [SerializeField] private int current;
    [SerializeField] private int currentLiveTime = 0;
    [SerializeField] private int timeOnLive = 0;
    [SerializeField] private bool _onAir;
    private bool _isOnAir;
    
    [SerializeField] LevelTimer _levelTimer;
    [SerializeField] private GameObject _groupCamera;
    [SerializeField] private GameObject _tvShader;
    [SerializeField] private GameObject _tvHackedShader;
    [SerializeField] private TextMeshProUGUI[] _pointsText; 
    [SerializeField] private Image[] strikesImages;
    [SerializeField] private Image[] shieldStrikesImages;
    [SerializeField] private FlowerUI[] flowerImages;
    [SerializeField] private GameObject onLiveMenu;
    [SerializeField] private GameObject offLiveMenu;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject dogHacker;
    [SerializeField] private Image[] _padlocksImages;
    [SerializeField] private Animator _hacker;

    [SerializeField] private bool onTutorial = false;
    
    
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        menu.SetActive(false);
        current = _hackTimes.Count - 1;
        if (onTutorial)
        {
            _onAir = true;
            ActivateCamera();
            //EventManager.Instance.Trigger(EventType.OnLive);
            _levelTimer.OnLive();
        }
    }
    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnFinishGame);
    }

    public void PlayHackingWarning()
    {
        _hacker.Play("Appear");
    }
    public void EndHackingWarning()
    {
        _hacker.Play("Disappear");
    }
    private void OnFinishGame(object[] obj)
    {
        menu.SetActive(false);
        dogHacker.SetActive(false);
    }

    private void OnResumeGame(object[] obj)
    {
        Debug.Log("ResumeGame");
        menu.SetActive(true);
    }
    private void OnPauseGame(object[] obj)
    {
        menu.SetActive(false);
    }
    
    public void StartLiveCamera(bool status)
    {
        if (onTutorial) return;
        GoOnAir();
    }
    void GoOnAir()
    { 
        EventManager.Instance.Trigger(EventType.OnLive);
        if(SoundManager.instance) SoundManager.instance.Play(SoundsTypes.LockOpen);
        _onAir = true;
        ActivateCamera();
        _levelTimer.OnLive();
    }
    public void GoOffAir()
    {
        _onAir = false;
        DesactivateAllCameras();
        _levelTimer.OffLive();
        StartCoroutine(TimeUntilGoOnAir(_hackTime));
    }

    private IEnumerator TimeUntilGoOnAir(float time)
    {
        yield return new WaitForSeconds(time);
        GoOnAir();
    }

    private void ActivateCamera()
    {
        SetPadLocks(false);
        // onLiveMenu.SetActive(true);
        offLiveMenu.SetActive(false);
        _groupCamera.SetActive(true);
        //_tvShader.SetActive(true);
        //_tvHackedShader.SetActive(false);
    }

    private void DesactivateAllCameras()
    {
        SetPadLocks(true);
        // onLiveMenu.SetActive(false);
        offLiveMenu.SetActive(true);
        _groupCamera.SetActive(false);
        //_tvShader.SetActive(false);
        //_tvHackedShader.SetActive(true);
    }

    private void SetPadLocks(bool isActiva)
    {
        foreach (var item in _padlocksImages)
        {
            item.enabled = isActiva;
        }
    }

    public bool IsOnAir()
    {
        return _onAir;
    }
    public void GetTVShaders(GameObject tvShader)
    {
        _tvShader = tvShader;
    }
    public void GetTVHackedShaders(GameObject tvHackedShader)
    {
        _tvHackedShader = tvHackedShader;
    }
    public TextMeshProUGUI[] GetPointsText()
    {
        return _pointsText;
    }    
    public Image[] GetStrikesUI()
    {
        return strikesImages;
    }
    public Image[] GetShieldStrikesUI()
    {
        return shieldStrikesImages;
    }    
    public FlowerUI[] GetFlowerImagesUI()
    {
        return flowerImages;
    }

    public List<int> GetHackTimes() { return _hackTimes;}
    public void SetLevelTime(int time)
    {
        _levelTime = time;
        currentLiveTime = time;
    }

    public void StartTutorialHackCamera()
    {
        _onAir = false;
        DesactivateAllCameras();
        //EventManager.Instance.Trigger(EventType.OffLive);
        _levelTimer.OffLive();
        if(SoundManager.instance) SoundManager.instance.Play(SoundsTypes.Hacking,null, true);
        // _currentHackTime = 10;
        // StartCoroutine(TimeUntilGoOnAir(_currentHackTime));
    }

    public void StopTutorialHackCamera()
    {
        _onAir = true;
        ActivateCamera();
        if(SoundManager.instance) SoundManager.instance.Pause(SoundsTypes.Hacking);
        //EventManager.Instance.Trigger(EventType.OnLive);
        _levelTimer.OnLive();
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
}