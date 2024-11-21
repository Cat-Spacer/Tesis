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
    [SerializeField] private float _maxTimeOnAir;
    [SerializeField] private float _minTimeOnAir;
    [SerializeField] private float _timeOnAir;
    [SerializeField] private float _timeUntilOnAir;
    [SerializeField] private float _timeVariation ;
    
    [SerializeField] private bool _onAir;
    private bool _isOnAir;
    
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
    private void ChangeCameraType(object[] obj)
    {
        var state = (bool) obj[0];
        ActivateCamera();
    }
    
    public void StartLiveCamera(bool status)
    {
        ActivateCamera();
        GoOnAir();
    }

    float CalculateTimeOnAir()
    {
        int peace = PeaceSystem.instance.GetCurrentPeace();
        float newTime = Mathf.Lerp(_minTimeOnAir, _maxTimeOnAir, 1 - peace / 10f);
        float variation = newTime * _timeVariation;
        float liveTime = Random.Range(newTime - variation, newTime + variation);
        liveTime = Mathf.Clamp(liveTime, _minTimeOnAir, _maxTimeOnAir);
        return liveTime;
    }

    float CalculateTimeUntilAir()
    {
        int peace = PeaceSystem.instance.GetCurrentPeace();
        float newTime = Mathf.Lerp(_maxTimeOnAir, _minTimeOnAir, 1 - peace / 10f);
        float variation = newTime * _timeVariation;
        float liveTime = Random.Range(newTime - variation, newTime + variation);
        liveTime = Mathf.Clamp(liveTime, _minTimeOnAir, _maxTimeOnAir);
        return liveTime;
    }
    void GoOnAir()
    {
        _onAir = true;
        ActivateCamera();
        EventManager.Instance.Trigger(EventType.OnLive);
        StartCoroutine(OnAirTimer(CalculateTimeOnAir()));
    }
    IEnumerator OnAirTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GoOffAir();
    }
    public void GoOffAir()
    {
        _onAir = false;
        DesactivateAllCameras();
        EventManager.Instance.Trigger(EventType.OffLive);
        StartCoroutine(TimeUntilGoOnAir(CalculateTimeUntilAir()));
    }
    
    IEnumerator TimeUntilGoOnAir(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GoOnAir();
    }

    void ActivateCamera()
    {
        if (!_onAir) return;
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
}
