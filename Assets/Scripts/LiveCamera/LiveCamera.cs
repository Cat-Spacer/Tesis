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
    [SerializeField] private GameObject[] _tvShader;
    [SerializeField] private GameObject[] _tvHackedShader;
    [SerializeField] private TextMeshProUGUI[] _pointsText; 
    [SerializeField] private Image[] strikesImages;
    [SerializeField] private Image[] shieldStrikesImages;
    [SerializeField] private Image[] flowerImages;
    [SerializeField] private GameObject onLiveMenu;
    [SerializeField] private GameObject offLiveMenu;
    [SerializeField] private GameObject menu;

    [SerializeField] private bool onTutorial = false;
    
    
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        current = _hackTimes.Count - 1;
        if (onTutorial)
        {
            _onAir = true;
            ActivateCamera();
            EventManager.Instance.Trigger(EventType.OnLive);
        }
        foreach(var cameraShader in _tvHackedShader) cameraShader.gameObject.SetActive(false);
    }

    private void OnFinishGame(object[] obj)
    {
        menu.SetActive(false);
    }

    private void OnResumeGame(object[] obj)
    {
        menu.SetActive(true);
    }
    private void OnPauseGame(object[] obj)
    {
        menu.SetActive(false);
    }
    
    public void StartLiveCamera(bool status)
    {
        if (onTutorial) return;
        ActivateCamera();
        GoOnAir();
    }
    void GoOnAir()
    { 
        _onAir = true;
        ActivateCamera();
        EventManager.Instance.Trigger(EventType.OnLive);
        if (current < 0) return; 
        // currentLiveTime = _levelTime - currentLiveTime - _hackTimes[current];
        // timeOnLive = currentLiveTime - _hackTimes[current];
        // _levelTime = _hackTimes[current];

        timeOnLive = currentLiveTime - _hackTimes[current];
        currentLiveTime = _hackTimes[current];
        StartCoroutine(OnAirTimer(timeOnLive));
    }
    IEnumerator OnAirTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (current >= 0)
        {
            current--;
            GoOffAir();
        }
    }
    public void GoOffAir()
    {
        _onAir = false;
        DesactivateAllCameras();
        EventManager.Instance.Trigger(EventType.OffLive);
        StartCoroutine(TimeUntilGoOnAir(_hackTime));
    }
    
    IEnumerator TimeUntilGoOnAir(float time)
    {
        yield return new WaitForSeconds(time);
        GoOnAir();
    }

    void ActivateCamera()
    {
        onLiveMenu.SetActive(true);
        offLiveMenu.SetActive(false);
        _groupCamera.SetActive(true);
        foreach (var shader in _tvShader) shader.SetActive(true);
        foreach (var shader in _tvHackedShader) shader.SetActive(false);
    }

    private void DesactivateAllCameras()
    {
        onLiveMenu.SetActive(false);
        offLiveMenu.SetActive(true);
        _groupCamera.SetActive(false);
        foreach (var shader in _tvShader) shader.SetActive(false);
        foreach (var shader in _tvHackedShader) shader.SetActive(true);
    }
    public bool IsOnAir()
    {
        return _onAir;
    }
    public void GetTVShaders(GameObject[] tvShaders)
    {
        _tvShader = tvShaders;
    }
    public void GetTVHackedShaders(GameObject[] tvHackedShaders)
    {
        _tvHackedShader = tvHackedShaders;
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
    public Image[] GetFlowerImagesUI()
    {
        return flowerImages;
    }
    public void SetLevelTime(int time)
    {
        _levelTime = time;
        currentLiveTime = time;
    }

    public void StartTutorialHackCamera()
    {
        _onAir = false;
        DesactivateAllCameras();
        EventManager.Instance.Trigger(EventType.OffLive);
        // _currentHackTime = 10;
        // StartCoroutine(TimeUntilGoOnAir(_currentHackTime));
    }

    public void StopTutorialHackCamera()
    {
        _onAir = true;
        ActivateCamera();
        EventManager.Instance.Trigger(EventType.OnLive);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
    }
}
