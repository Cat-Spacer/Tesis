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
    [SerializeField] private int _levelTime;
    [SerializeField] private int _hackCount;
    [SerializeField] private int _hackTime;
    [SerializeField] private float _currentHackTime;
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
    [SerializeField] private Slider[] sliders;
    [SerializeField] private GameObject onLiveMenu;
    [SerializeField] private GameObject offLiveMenu;
    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    private void Start()
    {
        current = _hackCount - 1;
    }

    private void ChangeCameraType(object[] obj)
    {
        var state = (bool) obj[0];
        ActivateCamera();
    }
    
    public void StartLiveCamera(bool status)
    {
        CalculateTimeOnAir();
        ActivateCamera();
        GoOnAir();
    }

    void CalculateTimeOnAir()
    {
        float liveTime = _levelTime - (_hackCount * _hackTime);
        float interval = liveTime / (_hackCount + 1);

        float timeAccumulated = 0;
        for (int i = 0; i < _hackCount; i++)
        {
            timeAccumulated += interval;
            _hackTimes.Add(Mathf.FloorToInt(timeAccumulated));
            timeAccumulated += _hackTime;
        }
    }

    void CalculateHackTime()
    {
        var currentPeace = PeaceSystem.instance.GetCurrentPeace();
        float adjustedHackTime = _hackTime * (1f + (currentPeace - 5f) / 10);
        adjustedHackTime = Mathf.Clamp(adjustedHackTime, 1f, _hackTime * 2);
        _currentHackTime = adjustedHackTime;
    }
    void GoOnAir()
    { 
        _onAir = true;
        ActivateCamera();
        EventManager.Instance.Trigger(EventType.OnLive);
        if (current < 0) return;
        timeOnLive = currentLiveTime - _hackTimes[current];
        currentLiveTime = _hackTimes[current];
        //currentLiveTime = _levelTime - currentLiveTime - _hackTimes[current];
        StartCoroutine(OnAirTimer(timeOnLive));
    }
    IEnumerator OnAirTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (current >= 0)
        {
            current--;
            Debug.Log("OnAir Entre");
            GoOffAir();
        }
    }
    public void GoOffAir()
    {
        _onAir = false;
        DesactivateAllCameras();
        EventManager.Instance.Trigger(EventType.OffLive);
        CalculateHackTime();
        StartCoroutine(TimeUntilGoOnAir(_currentHackTime));
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
    public Slider[] GetSliders()
    {
        return sliders;
    }

    public void SetLevelTime(int time, LevelTimer levelTimer)
    {
        _levelTime = time;
        currentLiveTime = time;
        _levelTimer = _levelTimer;
    }
}
